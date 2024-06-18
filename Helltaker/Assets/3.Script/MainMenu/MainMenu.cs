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
    //���� ���۽� �޽���
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
        //�� ����
        if (index == 0)
        {
            ToggleMenu(false);
            middleImage.texture = null;
            middleImage.color = upImage.color;
            DialogueManager.instance.GetInteractionEvent(newGame);
            DialogueManager.instance.startNewGame = true;
            levelName = newGame.dialogue.name;
            DialogueManager.instance.nextLevelName = levelName;
        }
        //é�� ����
        else if(index == 1)
        {

        }
        //���� ����
        else
        {
            DialogueManager.instance.GetInteractionEvent(exitGame);
        }

    }
}
