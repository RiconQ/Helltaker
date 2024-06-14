using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            dialogueParser = GetComponent<DialogueParser>();
            ParseDialogue();
            ParseSelect();

            //DontDestroyOnLoad(this);
        }

        else
        {
            Debug.Log("이미 이 게임에는 DatabaseManager 존재합니다.");
            Destroy(gameObject);
        }
    }

    [SerializeField] string dialogueCsvFileName;
    [SerializeField] string selectCsvFileName;
    Dictionary<int, Dialogue> dialogueDic = new Dictionary<int, Dialogue>();
    Dictionary<int, EventSelect> selectDic = new Dictionary<int, EventSelect>();

    public bool isParseDialogueFinish = false;
    public bool isParseSelectFinish = false;
    DialogueParser dialogueParser;
    EventSelectParser selectParser;



    public void ParseDialogue()
    {
        dialogueParser = transform.GetComponent<DialogueParser>();
        Dialogue[] dialogues = dialogueParser.Parse(dialogueCsvFileName);
        for (int i = 0; i < dialogues.Length; i++)
        {
            dialogueDic.Add(i + 1, dialogues[i]);
            //Debug.Log(i+1);
        }
        //Debug.Log("Parse Dialogue Done, dialogueDic : " + dialogueDic.Count);
        isParseDialogueFinish = true;
    }

    public void ParseSelect()
    {
        selectParser = transform.GetComponent<EventSelectParser>();
        EventSelect[] selects = selectParser.Parse(selectCsvFileName);

        for (int i = 0; i < selects.Length; i++)
        {
            //Debug.Log($"Select Dic Add : {int.Parse(selects[i].ID)}");
            selectDic.Add(int.Parse(selects[i].ID), selects[i]);
        }
        //Debug.Log("Parse Select Done, selectDic : " + selectDic.Count);
        isParseSelectFinish = true;
    }

    public Dialogue[] GetDialogues(int startNum, int endNum)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();
        for (int i = 0; i <= endNum - startNum; i++)
        {
            dialogueList.Add(dialogueDic[startNum + i]);
        }

        return dialogueList.ToArray();
    }


    public EventSelect[] GetSelects(int startNum, int endNum)
    {
        List<EventSelect> selectList = new List<EventSelect>();
        for (int i = 0; i <= endNum - startNum; i++)
        {
            selectList.Add(selectDic[startNum + i]);
        }

        return selectList.ToArray();
    }
}
