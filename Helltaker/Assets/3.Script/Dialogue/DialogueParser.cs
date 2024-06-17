using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public Dialogue[] Parse(string csvFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();
        TextAsset csvData = Resources.Load<TextAsset>(csvFileName);

        string[] data = csvData.text.Split(new char[] { '\n' });

        //Debug.Log(data.Length);
        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });

            Dialogue dialogue = new Dialogue();
            dialogue.name = row[1];
            //Debug.Log($"{i} : {row[1]}");

            List<string> contextList = new List<string>();
            List<string> eventNum = new List<string>();
            List<string> skipLine = new List<string>();
            List<string> showDeath = new List<string>();
            List<string> clearStage = new List<string>();
            List<string> portrait = new List<string>();
            List<string> animation = new List<string>();

            while(true)
            {
                contextList.Add(row[2]);
                eventNum.Add(row[3]);
                skipLine.Add(row[4]);
                showDeath.Add(row[5]);
                clearStage.Add(row[6]);
                portrait.Add(row[7]);
                animation.Add(row[8]);
                //Debug.Log(row[2]);
                if (i + 1 < data.Length)
                {
                    i++;

                    row = data[i].Split(new char[] { ',' });
                    if (row[0].ToString() != "")
                    {
                        i--;
                        break;
                    }
                }
                else break;
            }
            dialogue.eventNum = eventNum.ToArray();
            dialogue.skipLine = skipLine.ToArray();
            dialogue.contexts = contextList.ToArray();
            dialogue.showDeath = showDeath.ToArray();
            dialogue.clearStage = clearStage.ToArray();
            dialogue.portrait = portrait.ToArray();
            dialogue.animation = animation.ToArray();

            dialogueList.Add(dialogue);
        }

        //Debug.Log(dialogueList.Count);
        return dialogueList.ToArray();
    }

    //private void Start()
    //{
    //    Parse("Dialogue/DialogueEvent");
    //}
}
