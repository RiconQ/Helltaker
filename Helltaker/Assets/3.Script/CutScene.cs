using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    [SerializeField] private Goal goal;

    public void StartCutScene()
    {
        goal.StartDialogue();
    }
}
