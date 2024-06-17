using System;
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
    [SerializeField] private GameObject clearUI;
    [SerializeField] private GameObject dialogueBG;
    [SerializeField] private Image portrait;

    [SerializeField] private Animator fadeOutAnimator;

    [SerializeField] private Sprite[] portraitArray;
    [SerializeField] private Animator portraitAnimator;
    [SerializeField] private AnimationClip[] portraitAnimations;
    [SerializeField] private Sprite[] animThumnail;

    private int lineX;
    private Dialogue[] dialogues;
    private EventSelect[] selects;
    public bool isDialogue = false; //대화중이면 true
    private bool isNext = false; // 특정 키 입력 대기
    private bool isSelect = false; // 선택지 있을 시 키 입력 대기

    private int lineCount; //대화 라인 카운트 -> csv에서 ID
    private int contextCount; //대사 카운트

    [SerializeField] private GameObject selectUI;
    [SerializeField] private Button[] selectBox;

    [SerializeField] private int eventID = -1;

    public string nextLevelName;

    [SerializeField] private bool isBoss = false; // 보스전일 경우 대화 끝나면 다음 씬으로

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
                //if (isSelect)
                //{
                //    //Debug.Log("isSelect True");
                //    //if(Input.GetKeyDown(KeyCode.UpArrow))
                //    //{
                //    //
                //    //}
                //    //else if(Input.GetKeyDown(K))
                //}
                if (!isSelect)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        isNext = false;
                        txtDialogue.text = "";
                        txtName.text = "";

                        Debug.Log($"line {lineCount}, context {contextCount}");

                        // 베드 엔딩 선택지를 선택할 경우
                        if (deathUI.activeSelf)
                        {
                            EndDialogue();
                            return;
                        }
                        if (clearUI.activeSelf)
                        {
                            Debug.Log("Clear");
                            EndDialogue();
                            return;
                        }

                        // 베드 엔딩 선택지를 선택할 경우
                        if (dialogues[lineCount].showDeath[0] != ("")
                            && contextCount == dialogues[lineCount].contexts.Length - 2)
                        {
                            if (!deathUI.activeSelf)
                            {
                                Debug.Log("ShowDeath");
                                ShowDeath();
                            }
                        }

                        // 선택지 있음.
                        Debug.Log($"선택지 직전 {lineCount}, {contextCount}");
                        //Debug.Log(dialogues[lineCount].eventNum.Length);
                        int tmpEvent = dialogues[lineCount].eventNum.Length - 1;
                        if (!dialogues[lineCount].eventNum[tmpEvent].Equals("") && contextCount == tmpEvent)
                        {
                            Debug.Log($"선택지 출현 {lineCount}, {contextCount}");
                            //Debug.Log(dialogues[lineCount].eventNum[0]);
                            ShowSelect(dialogues[lineCount].eventNum[tmpEvent]);
                            isSelect = true;
                            //contextCount -= 1;
                            isNext = true;
                            //selectIndex = 0;
                            SetDialogue();
                            return;
                        }
                        if (contextCount + 1 < dialogues[lineCount].contexts.Length)
                        {
                            contextCount += 1;
                            Debug.Log("스킵 없음, 다음 Context");
                            if (deathUI.activeSelf)
                                SetDialogue(Color.red, false);
                            else
                                SetDialogue();
                        }
                        else
                        {
                            if (!dialogues[lineCount].skipLine[0].Equals(""))
                            {
                                Debug.Log($"스킵 라인 있음 {int.Parse(dialogues[lineCount].skipLine[0]) - lineX}");
                                if (dialogues[lineCount].clearStage[0] != ""
                                    && contextCount == dialogues[lineCount].contexts.Length - 1)
                                {
                                    contextCount = 0;
                                    //contextCount -= 1;
                                    SetDialogue();
                                    if (!clearUI.activeSelf)
                                        ShowClear();
                                    isNext = true;
                                    return;
                                }
                                lineCount = int.Parse(dialogues[lineCount].skipLine[0]) - lineX;
                                //Debug.Log(lineCount);
                                //if (lineCount + 1 < dialogues.Length)
                                //{
                                //    lineCount += 1;
                                //    SetDialogue();
                                //}
                            }
                            if (lineCount + 1 < dialogues.Length)
                            {
                                lineCount += 1;
                                contextCount = 0;
                                Debug.Log($"스킵 없음, 다음 Line : Line : {lineCount}  Context : {contextCount}");
                                if (dialogues[lineCount].clearStage[0] != ""
                                    && contextCount == dialogues[lineCount].contexts.Length - 1)
                                {
                                    //contextCount -= 1;
                                    SetDialogue();
                                    if (!clearUI.activeSelf)
                                        ShowClear();
                                    isNext = true;
                                    return;
                                }
                                if (deathUI.activeSelf)
                                    SetDialogue(Color.red);
                                else
                                    SetDialogue();
                            }
                            else
                            {

                                //대화 종료
                                // 대화 종료시 상황

                                // 2. 스테이지 클리어 -> 다음 레벨로
                                Debug.Log($"대화 끝 line : {lineCount}, context : {contextCount}");
                                //Debug.Log($"대화 끝 {dialogues[lineCount].contexts.Length - 1}");
                                //Debug.Log($"대화 끝 {(dialogues[lineCount].clearStage[0]!="")}");

                                //int tmpClear = dialogues[lineCount].contexts.Length - 1;
                                if (dialogues[lineCount].clearStage[0] != ""
                                    && contextCount == dialogues[lineCount].contexts.Length - 1)
                                {
                                    //contextCount -= 1;
                                    SetDialogue();
                                    if (!clearUI.activeSelf)
                                        ShowClear();
                                    isNext = true;
                                    return;
                                }

                                // 3. 스테이지 실패 -> 사망 컷신 후 레벨 재시작
                                //ShowDeath();

                                // 1. 게임 도중 -> 게임 다시 진행
                                if (isBoss)
                                {
                                    GameManager.instance.NextLevel(nextLevelName);
                                }
                                EndDialogue();
                            }
                        }

                    }
                }
            }
        }
    }

    //InteractionEvent에 Dialogue 할당
    public void GetInteractionEvent(InteractionEvent interactionEvent)
    {
        portraitArray = interactionEvent.GetPortrait();
        ShowDialogue(interactionEvent.GetDialogue());
        selects = interactionEvent.GetEventSelects();
        lineX = interactionEvent.GetLineX();
        //ShowDialogue(GameObject.FindGameObjectWithTag("Goal").GetComponent<InteractionEvent>().GetDialogue());
        //selects = GameObject.FindGameObjectWithTag("Goal").GetComponent<InteractionEvent>().GetEventSelects();
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
        this.eventID = FindSelectID(eventID);
        Debug.Log($"eventID : {this.eventID}");
        Debug.Log($"select Length : {selects[this.eventID].select.Length}");

        ShowSelectUI(selects[this.eventID].select.Length, true);
        //for (int i = 0; i < selects[this.eventID].select.Length; i++)
        //{
        //    Debug.Log($"{selects[this.eventID].select[i]}, {selects[this.eventID].lineToMove[i]}");
        //}
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
            int tmpIndex = dialogues[lineCount].eventNum.Length - 1;
            string tmp =
                selects[FindSelectID(dialogues[lineCount].eventNum[tmpIndex])].select[i];
            tmp.Replace('`', ',');
            tmp.Replace('|', '\n');
            selectBox[i].gameObject.GetComponentInChildren<Text>().text = tmp;
            //Debug.Log(i);
        }
        for (int i = num; i < selectBox.Length; i++)
        {
            if (selectBox[i].gameObject.activeSelf)
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

        //Debug.Log("Sprite : " + int.Parse(dialogues[lineCount].portrait[contextCount]));
        if (int.Parse(dialogues[lineCount].portrait[contextCount]) != -1)
        {
            portrait.gameObject.SetActive(true);
            Debug.Log($"image num : {int.Parse(dialogues[lineCount].portrait[contextCount])}");
            try
            {
                Destroy(portrait.gameObject.GetComponent<Animator>());
            }
            catch { }
            portrait.sprite = portraitArray[int.Parse(dialogues[lineCount].portrait[contextCount])];
            portrait.SetNativeSize();
        }
        else
            portrait.gameObject.SetActive(false);
        if (dialogues[lineCount].animation[contextCount] != "")
        {
            int index = int.Parse(dialogues[lineCount].animation[contextCount]);
            //portraitAnimator.gameObject.GetComponent<Image>().enabled = true;
            //portraitAnimator.gameObject.SetActive(true);
            portraitAnimator.enabled = true;
            portraitAnimator.gameObject.GetComponent<Image>().sprite = animThumnail[index];
            portraitAnimator.gameObject.GetComponent<Image>().SetNativeSize();
            portraitAnimator.Play(portraitAnimations[index].name);
        }
        else
        {
            //portraitAnimator.enabled = false;
            portraitAnimator.gameObject.GetComponent<Image>().enabled = false;

        }
        //portraitAnimator.gameObject.SetActive(false);



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
            //fadeOutAnimator.gameObject.SetActive(true);
            //LevelManager.instance.SetNextLevelName();
            GameManager.instance.RestartLevel();
        }

        // 스테이지 클리어시
        if (clearUI.activeSelf)
        {
            GameManager.instance.NextLevel(nextLevelName);
        }

        isDialogue = false;
        contextCount = 0;
        lineCount = 0;
        dialogues = null;
        isNext = false;
        ToggleUI(false);

    }

    public void ToggleUI(bool value)
    {
        dialogueUI.SetActive(value);
    }

    private void ShowDeath()
    {
        dialogueBG.SetActive(false);
        deathUI.SetActive(true);
    }

    private void ShowClear()
    {
        Debug.Log("ShowClear");
        clearUI.SetActive(true);
    }
    public void ChoiceSelects(int index)
    {
        lineCount = int.Parse(selects[eventID].lineToMove[index]) - lineX;
        Debug.Log($"lineX : {lineX}, line to move : " + (int.Parse(selects[eventID].lineToMove[index]) - lineX));
        contextCount = -1;
        ShowSelectUI(0);
        isSelect = false;
    }
}
