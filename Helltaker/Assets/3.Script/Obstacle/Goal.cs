using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string levelName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().SetIsMoving(true);
        //GameManager.instance.usingTurn = false;

        // ���̾�α� ���
        DialogueManager.instance.GetInteractionEvent();

        //animator.gameObject.SetActive(true);
        //LevelManager.instance.SetNextLevelName(levelName);
    }
}
