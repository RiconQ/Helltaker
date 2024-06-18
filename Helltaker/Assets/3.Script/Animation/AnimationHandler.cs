using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    //����

    public void OnEndFadeIn()
    {
        TryStartCutScene();
        gameObject.SetActive(false);
    }
    private void TryStartCutScene()
    {
        try
        {
            GameObject.FindGameObjectWithTag("CutScene").GetComponent<CutScene>().StartCutScene();
        }
        catch { }
    }

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
    {
        lucyFace.SetActive(true);
        GetComponent<Animator>().enabled = false;
    }

    public void ShowSkel(int index)
        => Skel[index].SetActive(true);

    public void startLucyDialogue()
    {
        LucyGoal.StartDialogue();
        transform.gameObject.SetActive(false);
    }

    public void HideFX()
        => this.gameObject.SetActive(false);

    public void Victory()
        => GameManager.instance.NextLevel(DialogueManager.instance.nextLevelName);

    public void PlayVictoryAnim()
    {
        HeartParticleManager.instance.PlayVictoryAnim();
    }

    public void PlayHeartAnim()
    {
        HeartParticleManager.instance.PlayHeart();
    }

    public void PlayCerberus()
    {
        this.GetComponent<ParticleSystem>().Play();
    }
}
