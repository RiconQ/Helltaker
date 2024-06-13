using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] private DialogueEvent dialogue;
    [SerializeField] private SelectClass selectClass;
    public Dialogue[] GetDialogue()
    {
        dialogue.dialogues = 
            DatabaseManager.instance.GetDialogues(dialogue.line.x, dialogue.line.y);
        return dialogue.dialogues;
    }

    public EventSelect[] GetEventSelects()
    {
        selectClass.selects =
            DatabaseManager.instance.GetSelects(selectClass.line.x, selectClass.line.y);
        return selectClass.selects;
    }
}
