using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSelectParser : MonoBehaviour
{
    public EventSelect[] Parse(string csvFileName)
    {
        List<EventSelect> selectList = new List<EventSelect>();
        TextAsset csvData = Resources.Load<TextAsset>(csvFileName);

        string[] data = csvData.text.Split(new char[] { '\n' });

        for(int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });

            EventSelect eventSelect = new EventSelect();
            List<string> select = new List<string>();
            List<string> lineToMove = new List<string>();

            eventSelect.ID = row[0];

            while(true)
            {
                select.Add(row[1]);
                lineToMove.Add(row[2]);

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
            eventSelect.select = select.ToArray();
            eventSelect.lineToMove = lineToMove.ToArray();

            selectList.Add(eventSelect);

        }

        return selectList.ToArray();
    }
}
