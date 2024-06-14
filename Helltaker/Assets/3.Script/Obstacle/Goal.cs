using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string levelName;
    [SerializeField] private bool isLucy;
    [SerializeField] private bool isHome;
    [SerializeField] private GameObject lucyObject;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().SetIsMoving(true);
        //GameManager.instance.usingTurn = false;

        //루시 애니메이션 전용
        if (isLucy)
        {
            DialogueManager.instance.ToggleUI(true);
            lucyObject.SetActive(true);
        }
        else if(isHome)
        {
            StartDialogue();
            transform.parent.gameObject.SetActive(false);
        }

        else
        {
            // 다이얼로그 출력
            StartDialogue();

        }
        //animator.gameObject.SetActive(true);
        //LevelManager.instance.SetNextLevelName(levelName);
    }
    
    public void StartDialogue()
    {
        DialogueManager.instance.GetInteractionEvent(GetComponent<InteractionEvent>());
        DialogueManager.instance.nextLevelName = levelName;
    }
}
