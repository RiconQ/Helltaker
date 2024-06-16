using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    //����

    public void OnEndFadeIn()
        => gameObject.SetActive(false);

    public void LoadNextLevel()
        => LevelManager.instance.LoadNextLevel();

    public void OnEndDeath()
        => GameManager.instance.RestartLevel();

    public void OnGetKey()
        => Destroy(gameObject);
    

    // �����
    [SerializeField] private GameObject lucyFace;
    [SerializeField] private GameObject[] Skel;
    [SerializeField] private Goal LucyGoal;


    public void MeetLucy()
        => lucyFace.SetActive(true);

    public void ShowSkel(int index)
        => Skel[index].SetActive(true);

    public void startLucyDialogue()
    {
        LucyGoal.StartDialogue();
        transform.gameObject.SetActive(false);
    }

    public void HideFX()
        => this.gameObject.SetActive(false);
}
