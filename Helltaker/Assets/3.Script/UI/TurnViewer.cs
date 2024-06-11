using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnViewer : MonoBehaviour
{
    private Text turnText;

    private void Awake()
    {
        turnText = transform.GetComponent<Text>();
    }

    private void FixedUpdate()
    {
        
        turnText.text = GameManager.instance.RemainTurn.ToString();
    }
}
