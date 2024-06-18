using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject buttonUI;
    [SerializeField] private Button[] menuButtons;

    [SerializeField] private InteractionEvent newGame;
    [SerializeField] private InteractionEvent exitGame;

    [SerializeField] private RawImage middleImage;
    [SerializeField] private Image upImage;

    [SerializeField] private string levelName = "";
    //게임 시작시 메시지
    private void Start()
    {
        DialogueManager.instance.GetInteractionEvent(GetComponent<InteractionEvent>());
    }

    public void ToggleMenu(bool value)
    {
        buttonUI.SetActive(value);
        foreach (var item in menuButtons)
            item.gameObject.SetActive(value);
        EventSystem.current.SetSelectedGameObject(menuButtons[0].gameObject);
    }


    public void ChoiceMenu(int index)
    {
        //새 게임
        if (index == 0)
        {
            ToggleMenu(false);
            middleImage.texture = null;
            middleImage.color = upImage.color;
            DialogueManager.instance.startNewGame = true;
            DialogueManager.instance.GetInteractionEvent(newGame, 0);
            levelName = newGame.dialogue.name;
            DialogueManager.instance.nextLevelName = levelName;
        }
        //챕터 선택
        else if (index == 1)
        {

        }
        //게임 종료
        else
        {
            Debug.Log("exit");
            ToggleMenu(false);
            DialogueManager.instance.exitGame = true;
            DialogueManager.instance.GetInteractionEvent(exitGame, 0);
        }

    }
}