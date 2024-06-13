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
            Debug.Log("�̹� �� ���ӿ��� DialogueManager �����մϴ�.");
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
    public bool isDialogue = false; //��ȭ���̸� true
    private bool isNext = false; // Ư�� Ű �Է� ���
    private bool isSelect = false; // ������ ���� �� Ű �Է� ���

    private int lineCount; //��ȭ ���� ī��Ʈ -> csv���� ID
    private int contextCount; //��� ī��Ʈ

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

                        // ���� ���� �������� ������ ��� -> �ϼ�
                        if (dialogues[lineCount].showDeath[0].Equals("TRUE"))
                        {
                            if (!deathUI.activeSelf)
                            {
                                Debug.Log("ShowDeath");
                                ShowDeath();
                            }
                        }

                        // ������ ����.
                        if (!dialogues[lineCount].eventNum[0].Equals(""))
                        {
                            ShowSelect(dialogues[lineCount].eventNum[0]);
                            isSelect = true;
                            //contextCount -= 1;
                            isNext = true;
                            selectIndex = 0;
                            SetDialogue();
                            Debug.Log($"������ ���� {lineCount}, {contextCount}");
                            return;
                        }
                        if (contextCount + 1 < dialogues[lineCount].contexts.Length)
                        {
                            contextCount += 1;
                            Debug.Log($"��ŵ ����, ���� Context : Line : {lineCount}  Context : {contextCount}");
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
                                Debug.Log($"��ŵ ���� ���� {dialogues[lineCount].skipLine[0]}");
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
                                //Debug.Log($"��ŵ ����, ���� Line : Line : {lineCount}  Context : {contextCount}");
                                if (deathUI.activeSelf)
                                    SetDialogue(Color.red);
                                else
                                    SetDialogue();
                            }
                            else
                            {

                                //��ȭ ����
                                // ��ȭ ����� ��Ȳ
                                // 1. ���� ���� -> ���� �ٽ� ����
                                //EndDialogue();
                                // 2. �������� Ŭ���� -> ���� ������
                                // 3. �������� ���� -> ��� �ƽ� �� ���� �����
                                //ShowDeath();

                            }
                        }
                    }
                }
            }
        }
    }

    //InteractionEvent�� Dialogue �Ҵ�
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
        // ������ �߸� �����Ͽ� ���� ������ �Ŷ�� 
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
