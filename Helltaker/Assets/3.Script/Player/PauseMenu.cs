using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    public bool gamePause = false;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private Goal goal;

    public void TogglePause(bool value)
    {
        pauseUI.SetActive(value);

        //일시 정지
        if (value)
        {
            DialogueManager.instance.isDialogue = false;
            Time.timeScale = 0;
            EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
        }

        //게임 재개
        else
        {
            Time.timeScale = 1;
        }
    }

    public void ResumeGame()
    {
        gamePause = false;
        TogglePause(false);
    }

    public void SkipPuzzle(bool isBoss)
    {
        
        TogglePause(false);
        if (isBoss)
        {
            gamePause = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<BossPlayer>().isCheat = true;
            //하트 파티클 생성
            return;
        }
        DialogueManager.instance.isSkip = true;
        goal.StartDialogue();
    }
    public void ToMainMenu()
    {
        Time.timeScale = 1;
        GameManager.instance.NextLevel("MainMenu");
    }
}
