using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }

        else
        {
            Debug.Log("이미 이 게임에는 DialogueManager 존재합니다.");
            Destroy(gameObject);
        }
    }

    [SerializeField] private Text txtDialogue;
    [SerializeField] private Text txtName;

    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private GameObject deathUI;
    [SerializeField] private GameObject dialogueBG;

    [SerializeField] private Animator fadeOutAnimator;

    private Dialogue[] dialogues;
    private EventSelect[] selects;
    public bool isDialogue = false; //대화중이면 true
    private bool isNext = false; // 특정 키 입력 대기
    private bool isSelect = false; // 선택지 있을 시 키 입력 대기

    private int lineCount; //대화 라인 카운트 -> csv에서 ID
    private int contextCount; //대사 카운트

    [SerializeField] private GameObject selectUI;
    [SerializeField] private Button[] selectBox;

    [SerializeField] private int selectIndex = 0;

    private void Start()
    {
        ToggleUI(false);
    }

    private void Update()
    {

        if (isDialogue)
        {
            //Debug.Log("isDialogue True");
            if (isNext)
            {
                //Debug.Log("isNext True");
                if (isSelect)
                {
                    //Debug.Log("isSelect True");
                    //if(Input.GetKeyDown(KeyCode.UpArrow))
                    //{
                    //
                    //}
                    //else if(Input.GetKeyDown(K))
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        isNext = false;
                        txtDialogue.text = "";
                        txtName.text = "";

                        // 베드 엔딩 선택지를 선택할 경우 -> 완성
                        if (dialogues[lineCount].showDeath[0].Equals("TRUE"))
                        {
                            if (!deathUI.activeSelf)
                            {
                                Debug.Log("ShowDeath");
                                ShowDeath();
                            }
                        }

                        // 선택지 있음.
                        if (!dialogues[lineCount].eventNum[0].Equals(""))
                        {
                            ShowSelect(dialogues[lineCount].eventNum[0]);
                            isSelect = true;
                            //contextCount -= 1;
                            isNext = true;
                            selectIndex = 0;
                            SetDialogue();
                            Debug.Log($"선택지 출현 {lineCount}, {contextCount}");
                            return;
                        }
                        if (contextCount + 1 < dialogues[lineCount].contexts.Length)
                        {
                            contextCount += 1;
                            Debug.Log($"스킵 없음, 다음 Context : Line : {lineCount}  Context : {contextCount}");
                            if (deathUI.activeSelf)
                                SetDialogue(Color.red, false);
                            else
                                SetDialogue();
                        }
                        else
                        {
                            contextCount = 0;
                            if (!dialogues[lineCount].skipLine[0].Equals(""))
                            {
                                Debug.Log($"스킵 라인 있음 {dialogues[lineCount].skipLine[0]}");
                                lineCount = int.Parse(dialogues[lineCount].skipLine[0]) + 1;
                                Debug.Log(lineCount);
                                //if (lineCount + 1 < dialogues.Length)
                                //{
                                //    lineCount += 1;
                                //    SetDialogue();
                                //}
                            }
                            if (lineCount + 1 < dialogues.Length)
                            {
                                lineCount += 1;
                                //Debug.Log($"스킵 없음, 다음 Line : Line : {lineCount}  Context : {contextCount}");
                                if (deathUI.activeSelf)
                                    SetDialogue(Color.red);
                                else
                                    SetDialogue();
                            }
                            else
                            {

                                //대화 종료
                                // 대화 종료시 상황
                                // 1. 게임 도중 -> 게임 다시 진행
                                //EndDialogue();
                                // 2. 스테이지 클리어 -> 다음 레벨로
                                // 3. 스테이지 실패 -> 사망 컷신 후 레벨 재시작
                                //ShowDeath();

                            }
                        }
                    }
                }
            }
        }
    }

    //InteractionEvent에 Dialogue 할당
    public void GetInteractionEvent()
    {
        ShowDialogue(transform.GetComponent<InteractionEvent>().GetDialogue());
        selects = transform.GetComponent<InteractionEvent>().GetEventSelects();
    }

    public void ShowDialogue(Dialogue[] p_dialogues)
    {
        isDialogue = true;

        txtDialogue.text = "";
        txtName.text = "";
        dialogues = p_dialogues;

        SetDialogue();
        //for(int i = 0; i < dialogues.Length; i++)
        //{
        //    Debug.Log($"i : {i}, {dialogues[i].skipLine[i]}");
        //}
        Debug.Log($"Start : Line : {lineCount}  Context : {contextCount}");
    }

    public void ShowSelect(string eventID)
    {
        Debug.Log($"eventID : {eventID}");
        //Debug.Log($"select Length : {selects[FindSelectID(eventID)].select.Length}");
        ShowSelectUI(selects[FindSelectID(eventID)].select.Length, true);
    }
    public int FindSelectID(string eventID)
    {
        for (int i = 0; i < selects.Length; i++)
        {
            if (selects[i].ID.Equals(eventID))
                return i;
        }
        return -1;
    }

    // 아무 매개 변수 안건네주면 전부다 SetActive false;
    private void ShowSelectUI(int num = 3, bool value = false)
    {
        selectUI.SetActive(value);
        selectUI.GetComponent<RectTransform>().sizeDelta = new Vector2(400, num * 100);
        for (int i = 0; i < num; i++)
        {
            selectBox[i].gameObject.SetActive(value);
            //Debug.Log("test : " + selects[0].select.Length);
            // Debug.Log(selectBox[i].TryGetComponent(out test));
            string tmp =
                selects[FindSelectID(dialogues[lineCount].eventNum[0])].select[i];
            tmp.Replace('`', ',');
            tmp.Replace('|', '\n');
            selectBox[i].gameObject.GetComponentInChildren<Text>().text = tmp;
            Debug.Log(i);
        }
        for (int i = num; i < selectBox.Length; i++)
        {
            selectBox[i].gameObject.SetActive(false);
        }
        if (num != 3)
        {
            EventSystem.current.SetSelectedGameObject(selectBox[0].gameObject);
        }
    }


    private void SetDialogue(Color color = new Color(), bool showName = true)
    {
        if (color == new Color()) color = Color.white;
        ToggleUI(true);
        string currDialogue = dialogues[lineCount].contexts[contextCount];
        currDialogue = currDialogue.Replace('`', ',');
        currDialogue = currDialogue.Replace('|', '\n');

        txtName.text = (showName == true) ? dialogues[lineCount].name : "";
        txtName.color = Color.red;
        txtDialogue.text = currDialogue;
        txtDialogue.color = color;
        isNext = true;
    }

    private void EndDialogue()
    {
        // 선택지 잘못 선택하여 게임 오버된 거라면 
        if (deathUI.activeSelf)
        {
            fadeOutAnimator.gameObject.SetActive(true);
            LevelManager.instance.SetNextLevelName();
        }

        isDialogue = false;
        contextCount = 0;
        lineCount = 0;
        dialogues = null;
        isNext = false;
        ToggleUI(false);

    }

    private void ToggleUI(bool value)
    {
        dialogueUI.SetActive(value);
    }

    private void ShowDeath()
    {
        dialogueBG.SetActive(false);
        deathUI.SetActive(true);
    }
}
