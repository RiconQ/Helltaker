using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }

        else
        {
            Debug.Log("이미 이 게임에는 SceneManager 존재합니다.");
            Destroy(gameObject);
        }
    }

    //[SerializeField] private GameObject fadeIn;
    //[SerializeField] private GameObject fadeOut;
    //
    //
    //public void DeactiveFadeIn()
    //{
    //    fadeIn.SetActive(false);
    //}

    [SerializeField] private string levelName = " ";

    public void SetNextLevelName(string levelName)
    {
        this.levelName = levelName;
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(levelName);
    }
}
