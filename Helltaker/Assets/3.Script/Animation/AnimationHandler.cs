using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    
    public void OnEndFadeIn()
    {
        gameObject.SetActive(false);
    }

    public void LoadNextLevel()
    {
        LevelManager.instance.LoadNextLevel();
    }

    public void OnEndDeath()
    {
        GameManager.instance.RestartLevel();
    }
}
