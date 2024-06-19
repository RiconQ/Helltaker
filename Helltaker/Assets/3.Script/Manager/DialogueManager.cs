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
            //Debug.Log("�̹� �� ���ӿ��� DialogueManager �����մϴ�.");
            Destroy(gameObject);
        }
    }

    [Header("Text")]
    [SerializeField] private Text txtDialogue;
    [SerializeField] private Text txtName;

    [Header("UI")]
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private GameObject deathUI;
    [SerializeField] private GameObject clearUI;
    [SerializeField] private GameObject dialogueBG;
    [SerializeField] private Image portrait;
    [SerializeField] private GameObject booper;

    [Header("Animator")]
    [SerializeField] private Animator fadeOutAnimator;


    [Header("Portrait")]
    [SerializeField] private Sprite[] portraitArray;
    [SerializeField] private Animator portraitAnimator;
    //[SerializeField] private Animator portraitAnimator;
    //[SerializeField] private AnimationClip[] portraitAnimations;
    //[SerializeField] private Sprite[] animThumnail;
    private string tmpName = "";

    private int lineX;
    private Dialogue[] dialogues;
    private EventSelect[] selects;
    public bool isDialogue = false; //��ȭ���̸� true
    private bool isNext = false; // Ư�� Ű �Է� ���
    private bool isSelect = false; // ������ ���� �� Ű �Է� ���

    private int lineCount; //��ȭ ���� ī��Ʈ -> csv���� ID
    private int contextCount; //��� ī��Ʈ

    [SerializeField] private GameObject selectUI;
    [SerializeField] private Button[] selectBox;

    [SerializeField] private int eventID = -1;

    public string nextLevelName;
    private bool isClear = false;

    [Header("Exception")]
    [SerializeField] private bool isBoss = false; // �������� ��� ��ȭ ������ ���� ������\
    [SerializeField] private bool isLucy = false;
    [SerializeField] private GameObject lucyObj;
    [Header("Level Exception")]
    [SerializeField] private bool isLevel10 = false;
    [SerializeField] private bool isCutScene = false;
    public bool isHome = false;
    private bool isEnd = false;

    [Header("MainMenu")]
    public bool isMainMenu = false;
    [SerializeField] private MainMenu mainMenu;
    public bool startNewGame = false;
    public bool exitGame;
    [HideInInspector] public bool isSkip = false;


    private void Start()
    {
        if (isCutScene || isMainMenu)
            ToggleUI(true);
        else
            ToggleUI(false);
    }

    private void Update()
    {

        if (isSkip)
        {
            isSkip = false;
            return;
        }
        if (isDialogue)
        {
            if (isNext)
            {
                if (!isSelect)
                {
                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                    {

                        if (booper.activeSelf)
                            booper.GetComponent<Animator>().Play("booperPop");
                        isNext = false;
                        //txtDialogue.text = "";
                        //txtName.text = "";

                        //Debug.Log($"prev line {lineCount}, context {contextCount}");
                        if (clearUI.activeSelf)
                        {
                            clearUI.SetActive(false);
                        }

                        // 사망씬 활성화되어 있다면 클릭시 재시작
                        if (deathUI.activeSelf)
                        {
                            EndDialogue();
                            return;
                        }

                        //isClear가 true이고 마지막대사면 대화 종료, 클리어 애니메이션 재생
                        if (isClear && contextCount == dialogues[lineCount].contexts.Length - 1)
                        {
                            //Debug.Log("Clear");
                            EndDialogue();
                            return;
                        }

                        if (contextCount != -1)
                        {
                            //사망 트리거 존재하고, 현재 장면에서 사망 트리거 발동시 사망 애니메이션 재생
                            if (dialogues[lineCount].showDeath[contextCount] != (""))
                            {
                                if (!deathUI.activeSelf)
                                {
                                    //Debug.Log("ShowDeath");
                                    ShowDeath();
                                    //return;
                                }
                            }
                            if (dialogues[lineCount].clearStage[contextCount] != "")
                            {
                                // Debug.Log($"if prev line {lineCount}, context {contextCount}");
                                SetDialogue();
                                if (!clearUI.activeSelf && !isClear)
                                {
                                    //Debug.Log("ShowClear 135");
                                    ShowClear();
                                    isClear = true;
                                }
                            }
                        }
                        // 클리어 트리고 존재, 클리어 화면 비활성화시 애니메이션 재생
                        //Debug.Log($"prev line {lineCount}, context {contextCount}");
                        if (contextCount == -1)
                        {
                            //contextCount += 1;
                            //if (dialogues[lineCount].showDeath[contextCount] != (""))
                            //{
                            //    if (!deathUI.activeSelf)
                            //    {
                            //        Debug.Log("ShowDeath");
                            //        ShowDeath();
                            //        return;
                            //    }
                            //}
                            //else
                            //    contextCount -= 1;

                            contextCount += 1;
                            if (dialogues[lineCount].clearStage[contextCount] != "")
                            {
                                SetDialogue();
                                if (!clearUI.activeSelf && !isClear)
                                {

                                    //Debug.Log("ShowClear 164");
                                    ShowClear();
                                    isClear = true;
                                    return;
                                }
                            }
                            else
                                contextCount -= 1;
                        }
                        #region
                        //if (dialogues[lineCount].clearStage[contextCount == -1 ? 0 : contextCount] != ""
                        //            && contextCount == dialogues[lineCount].contexts.Length - 2)
                        //{
                        //    contextCount = 0;
                        //    SetDialogue();
                        //    if (!clearUI.activeSelf || !isClear)
                        //    {
                        //        ShowClear();
                        //        isClear = true;
                        //        return;
                        //    }
                        //    else
                        //        isNext = true;
                        //}
                        #endregion

                        //이벤트 발동 트리거
                        int tmpEvent = dialogues[lineCount].eventNum.Length - 1;
                        if (!dialogues[lineCount].eventNum[tmpEvent].Equals("") && contextCount == tmpEvent)
                        {
                            booper.SetActive(false);
                            ShowSelect(dialogues[lineCount].eventNum[tmpEvent]);
                            isSelect = true;
                            isNext = true;
                            SetDialogue();
                            return;
                        }
                        //line 그대로 -> 더 이어지는 context 존재
                        if (contextCount + 1 < dialogues[lineCount].contexts.Length)
                        {
                            //if (startNewGame || exitGame)
                            //    contextCount -= 1;

                            contextCount += 1;
                            if (deathUI.activeSelf)
                                SetDialogue(Color.red, false);
                            else
                                SetDialogue();
                        }
                        // 더 이어지는 context 없음
                        else
                        {
                            // skipLine 존재
                            if (!dialogues[lineCount].skipLine[0].Equals(""))
                            {
                                lineCount = int.Parse(dialogues[lineCount].skipLine[0]) - lineX;
                            }
                            // line 변경 후, 대화가 끝났는지 확인
                            if (lineCount + 1 < dialogues.Length)
                            {
                                lineCount += 1;
                                contextCount = 0;
                                if (deathUI.activeSelf)
                                    SetDialogue(Color.red);
                                else
                                    SetDialogue();
                            }
                            //대화 종료
                            else
                            {
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

    //InteractionEvent�� Dialogue �Ҵ�
    public void GetInteractionEvent(InteractionEvent interactionEvent, int line = 0, int context = 0)
    {
        lineCount = line;
        contextCount = context;
        portraitArray = interactionEvent.GetPortrait();
        ShowDialogue(interactionEvent.GetDialogue());
        selects = interactionEvent.GetEventSelects();
        lineX = interactionEvent.GetLineX();
        //ShowDialogue(GameObject.FindGameObjectWithTag("Goal").GetComponent<InteractionEvent>().GetDialogue());
        //selects = GameObject.FindGameObjectWithTag("Goal").GetComponent<InteractionEvent>().GetEventSelects();
    }

    public void ShowDialogue(Dialogue[] p_dialogues)
    {

        txtDialogue.text = "";
        txtName.text = "";
        dialogues = p_dialogues;

        SetDialogue();
        isDialogue = true;
        //for(int i = 0; i < dialogues.Length; i++)
        //{
        //    Debug.Log($"i : {i}, {dialogues[i].skipLine[i]}");
        //}
        //Debug.Log($"Start : Line : {lineCount}  Context : {contextCount}");
    }

    public void ShowSelect(string eventID)
    {
        //Debug.Log($"eventID : {eventID}");
        this.eventID = FindSelectID(eventID);
        // Debug.Log($"eventID : {this.eventID}");
        //Debug.Log($"select Length : {selects[this.eventID].select.Length}");

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

    // �ƹ� �Ű� ���� �Ȱǳ��ָ� ���δ� SetActive false;
    private void ShowSelectUI(int num = 2, bool value = false)
    {
        booper.SetActive(false);
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
            tmp = tmp.Replace('^', ',');
            Debug.Log(tmp);
            //tmp.Replace('|', '\n');
            selectBox[i].gameObject.GetComponentInChildren<Text>().text = tmp;
            //Debug.Log(i);
        }
        //for (int i = num; i < selectBox.Length; i++)
        //{
        //    if (selectBox[i].gameObject.activeSelf)
        //        selectBox[i].gameObject.SetActive(false);
        //}
        //if (num != 3)
        //{
        EventSystem.current.SetSelectedGameObject(selectBox[0].gameObject);
        //}
    }

    private void SetDialogue(Color color = new Color(), bool showName = true)
    {
        //isDialogue = false;
        Debug.Log($"Line : {lineCount}  Context : {contextCount}");
        //Debug.Log($"prev name {tmpName}, curr name {dialogues[lineCount].name}");
        if (color == new Color()) color = Color.white;
        ToggleUI(true);
        string currDialogue = dialogues[lineCount].contexts[contextCount];
        currDialogue = currDialogue.Replace('^', ',');
        currDialogue = currDialogue.Replace('|', '\n');
        portrait.gameObject.SetActive(false);

        portrait.color = new Color(1, 1, 1, 1);

        //Debug.Log("Sprite : " + int.Parse(dialogues[lineCount].portrait[contextCount]));
        if (int.Parse(dialogues[lineCount].portrait[contextCount]) != -1)
        {
            if (dialogues[lineCount].name != tmpName)
            {
                //Debug.Log($"prev name {tmpName}, curr name {dialogues[lineCount].name}");
                tmpName = dialogues[lineCount].name;
                portrait.gameObject.SetActive(true);
            }
            portrait.gameObject.SetActive(true);

            portrait.sprite = portraitArray[int.Parse(dialogues[lineCount].portrait[contextCount])];
            portrait.SetNativeSize();
        }
        else
        {
            //Debug.Log("portrait false");
            //portrait.sprite = null;
            portrait.color = new Color(0, 0, 0, 0);
        }
        #region
        //if (dialogues[lineCount].animation[contextCount] != "")
        //{
        //    int index = int.Parse(dialogues[lineCount].animation[contextCount]);
        //    //portraitAnimator.gameObject.GetComponent<Image>().enabled = true;
        //    //portraitAnimator.gameObject.SetActive(true);
        //    portraitAnimator.enabled = true;
        //    portraitAnimator.gameObject.GetComponent<Image>().sprite = animThumnail[index];
        //    portraitAnimator.gameObject.GetComponent<Image>().SetNativeSize();
        //    portraitAnimator.Play(portraitAnimations[index].name);
        //}
        //else
        //{
        //    //portraitAnimator.enabled = false;
        //    portraitAnimator.gameObject.GetComponent<Image>().enabled = false;
        //
        //}
        //portraitAnimator.gameObject.SetActive(false);

        #endregion

        if (showName == true)
        {
            if (!txtName.text.Equals(dialogues[lineCount].name))
            {
                //이름 다름. 애니메이션 출력
                //Debug.Log($"prev name {txtName.text}, curr name {dialogues[lineCount].name}");
                portraitAnimator.Play("PortraitFadeIn");
            }
            txtName.text = dialogues[lineCount].name;
        }
        else
        {
            txtName.text = "";
        }
        //portraitAnimator.Play("PortraitFadeIn");
        //Debug.Log($"prev name {txtName.text}, curr name {dialogues[lineCount].name}");
        txtName.text = (showName == true) ? dialogues[lineCount].name : "";
        txtName.color = Color.red;
        if (txtDialogue.text != currDialogue)
        {
            txtDialogue.text = currDialogue;
            txtDialogue.GetComponent<Animator>().Play("PopUp");
        }
        else
            txtDialogue.text = currDialogue;
        txtDialogue.color = color;
        isNext = true;
        if (!isSelect)
            booper.SetActive(true);
        isDialogue = true;
    }

    private void EndDialogue()
    {
        booper.SetActive(false);
        if (deathUI.activeSelf)
        {
            if (isLevel10)
            {
                GameManager.instance.NextLevel(nextLevelName);
                return;
            }
            if (isLucy) lucyObj.SetActive(false);
            //fadeOutAnimator.gameObject.SetActive(true);
            //LevelManager.instance.SetNextLevelName();
            GameManager.instance.RestartLevel();
        }

        if (isClear)
        {
            if (isLucy) lucyObj.SetActive(false);
            clearUI.SetActive(false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().SetIsMoving(true);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().Play("Victory");
            //GameManager.instance.NextLevel(nextLevelName);
        }
        if (isCutScene || isEnd)
        {
            GameManager.instance.NextLevel(nextLevelName);
            return;
        }
        //if(isEnd)
        //{
        //    Debug.Log("isEnd");
        //    GameManager.instance.NextLevel("MainMenu");
        //    return;
        //}

        txtDialogue.text = "";

        if (isMainMenu)
        {
            if (startNewGame)
            {
                GameManager.instance.NextLevel(nextLevelName);

                isDialogue = false;

                ToggleUI(false);
                return;
            }
            if (exitGame)
            {
                Application.Quit();
                return;
            }
            //메뉴 선택
            mainMenu.ToggleMenu(true);
            return;
        }
        txtName.text = "";

        isDialogue = false;
        contextCount = 0;
        lineCount = 0;
        dialogues = null;
        isNext = false;

        ToggleUI(false);

    }

    public void ToggleUI(bool value)
    {
        if (!value) dialogueUI.GetComponent<Animator>().Play("DialogueUIFadeOut");
        else dialogueUI.SetActive(value);
    }

    private void ShowDeath()
    {
        dialogueBG.SetActive(false);
        if (isLucy) lucyObj.SetActive(false);
        deathUI.SetActive(true);
        CameraShakeManager.instance.Shake();
    }

    private void ShowClear()
    {
        clearUI.SetActive(true);
    }
    public void ChoiceSelects(int index)
    {
        if (isHome && index == 0) isEnd = true;
        lineCount = int.Parse(selects[eventID].lineToMove[index]) - lineX;
        //Debug.Log($"lineX : {lineX}, line to move : " + (int.Parse(selects[eventID].lineToMove[index]) - lineX));
        contextCount = -1;
        ShowSelectUI(0);
        isSelect = false;
    }
}
