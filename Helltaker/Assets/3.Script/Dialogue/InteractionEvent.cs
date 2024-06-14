using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] private DialogueEvent dialogue;
    [SerializeField] private SelectClass selectClass;
    [SerializeField] private Sprite[] portraits;
    //[SerializeField] private string levelName;

    public Dialogue[] GetDialogue()
    {
        dialogue.dialogues =
            DatabaseManager.instance.GetDialogues(dialogue.line.x, dialogue.line.y);
        return dialogue.dialogues;
    }

    public int GetLineX()
    {
        return dialogue.line.x;
    }
    public EventSelect[] GetEventSelects()
    {
        if (selectClass.line != Vector2Int.zero)
            selectClass.selects =
                DatabaseManager.instance.GetSelects(selectClass.line.x, selectClass.line.y);
        return selectClass.selects;
    }

    public Sprite[] GetPortrait()
    {
        Debug.Log("Get portrait length: " + portraits.Length);
        return portraits;
    }
}
