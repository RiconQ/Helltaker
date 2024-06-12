using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    
    public void OnEndFadIn()
    {
        gameObject.SetActive(false);
    }

    public void LoadNextLevel()
    {
        LevelManager.instance.LoadNextLevel();
    }
}
