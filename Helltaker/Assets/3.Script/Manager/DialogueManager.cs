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
            Debug.Log("�̹� �� ���ӿ��� DialogueManager �����մϴ�.");
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
    //[SerializeField] private Animator portraitAnimator;
    //[SerializeField] private AnimationClip[] portraitAnimations;
    //[SerializeField] private Sprite[] animThumnail;
    private string tmpName ="";

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

    [SerializeField] private bool isBoss = false; // �������� ��� ��ȭ ������ ���� ������\
    [SerializeField] private bool isLucy = false;
    [SerializeField] private GameObject lucyObj;

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

                        // ���� ���� �������� ������ ���
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

                        // ���� ���� �������� ������ ���
                        if (dialogues[lineCount].showDeath[0] != ("")
                            && contextCount == dialogues[lineCount].contexts.Length - 2)
                        {
                            if (!deathUI.activeSelf)
                            {
                                Debug.Log("ShowDeath");
                                ShowDeath();
                            }
                        }

                        // ������ ����.
                        Debug.Log($"������ ���� {lineCount}, {contextCount}");
                        //Debug.Log(dialogues[lineCount].eventNum.Length);
                        int tmpEvent = dialogues[lineCount].eventNum.Length - 1;
                        if (!dialogues[lineCount].eventNum[tmpEvent].Equals("") && contextCount == tmpEvent)
                        {
                            Debug.Log($"������ ���� {lineCount}, {contextCount}");
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
                            Debug.Log("��ŵ ����, ���� Context");
                            if (deathUI.activeSelf)
                                SetDialogue(Color.red, false);
                            else
                                SetDialogue();
                        }
                        else
                        {
                            if (!dialogues[lineCount].skipLine[0].Equals(""))
                            {
                                Debug.Log($"��ŵ ���� ���� {int.Parse(dialogues[lineCount].skipLine[0]) - lineX}");
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
                                Debug.Log($"��ŵ ����, ���� Line : Line : {lineCount}  Context : {contextCount}");
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

                                //��ȭ ����
                                // ��ȭ ����� ��Ȳ

                                // 2. �������� Ŭ���� -> ���� ������
                                Debug.Log($"��ȭ �� line : {lineCount}, context : {contextCount}");
                                //Debug.Log($"��ȭ �� {dialogues[lineCount].contexts.Length - 1}");
                                //Debug.Log($"��ȭ �� {(dialogues[lineCount].clearStage[0]!="")}");

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

                                // 3. �������� ���� -> ��� �ƽ� �� ���� �����
                                //ShowDeath();

                                // 1. ���� ���� -> ���� �ٽ� ����
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

    // �ƹ� �Ű� ���� �Ȱǳ��ָ� ���δ� SetActive false;
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
            if (dialogues[lineCount].name != tmpName)
            {
                Debug.Log($"prev name {tmpName}, curr name {dialogues[lineCount].name}");
                tmpName = dialogues[lineCount].name;
                portrait.gameObject.SetActive(false);
                portrait.gameObject.SetActive(true);
            }
            portrait.gameObject.SetActive(true);
            Debug.Log($"image num : {int.Parse(dialogues[lineCount].portrait[contextCount])}");
            try
            {
                //Destroy(portrait.gameObject.GetComponent<Animator>());
            }
            catch { }
            portrait.sprite = portraitArray[int.Parse(dialogues[lineCount].portrait[contextCount])];
            portrait.SetNativeSize();
        }
        else
            portrait.gameObject.SetActive(false);
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


        //if(showName == true)
        //{
        //    if(!txtName.text.Equals(dialogues[lineCount].name))
        //    {
        //        //이름 다름. 애니메이션 출력
        //        Debug.Log($"prev name {txtName.text}, curr name {dialogues[lineCount].name}");
        //        portraitAnimator.Play("PortraitFadeIn");
        //    }
        //    txtName.text = dialogues[lineCount].name;
        //}
        //else
        //{
        //    txtName.text = "";
        //}
        #endregion
        //portraitAnimator.Play("PortraitFadeIn");
        //Debug.Log($"prev name {txtName.text}, curr name {dialogues[lineCount].name}");
        txtName.text = (showName == true) ? dialogues[lineCount].name : "";
        txtName.color = Color.red;
        txtDialogue.text = currDialogue;
        txtDialogue.color = color;
        isNext = true;
    }

    private void EndDialogue()
    {
        // ������ �߸� �����Ͽ� ���� ������ �Ŷ�� 
        if (deathUI.activeSelf)
        {
            if (isLucy) lucyObj.SetActive(false);
            //fadeOutAnimator.gameObject.SetActive(true);
            //LevelManager.instance.SetNextLevelName();
            GameManager.instance.RestartLevel();
        }

        // �������� Ŭ�����
        if (clearUI.activeSelf)
        {
            if (isLucy) lucyObj.SetActive(false);
            clearUI.SetActive(false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().Play("Victory");
            //GameManager.instance.NextLevel(nextLevelName);
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
        if (isLucy) lucyObj.SetActive(false);
        deathUI.SetActive(true);
        CameraShakeManager.instance.Shake();
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
