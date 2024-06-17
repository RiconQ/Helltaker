using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    private Animator playerAnim;
    private SpriteRenderer renderer;

    [SerializeField] private Animator runAnimator;
    [SerializeField] private Animator bloodAnimator;
    [SerializeField] private Animator hitAnimator;

    private void Awake()
    {
        renderer = transform.GetComponent<SpriteRenderer>();
        playerAnim = transform.GetComponent<Animator>();
    }
    public void FlipX(bool value)
        => renderer.flipX = value;

    #region PlayerAnim
    public void SetBoolPlayer(string key, bool value)
        => playerAnim.SetBool(key, value);

    public void SetTriggerPlayer(string trigger)
        => playerAnim.SetTrigger(trigger);
    #endregion


    //#region SkelAnim
    //public void SetTriggerSkel(Animator animator, string key)
    //    => 
    //
    //
    //#endregion

    public void ShowMoveFX(Vector3 position)
    {
        //Debug.Log("MoveFX");
        runAnimator.transform.position = position;
        runAnimator.gameObject.SetActive(true);
        int randomIndex = Random.Range(0, 3);
        switch(randomIndex)
        {
            case 0:
                runAnimator.Play("Move1");
                return;
            case 1:
                runAnimator.Play("Move2");
                return;
            case 2:
                runAnimator.Play("Move3");
                return;
        }
    }
    public void ShowBloodFX(Vector3 position)
    {
        bloodAnimator.transform.position = position;
        bloodAnimator.gameObject.SetActive(true);
        int randomIndex = Random.Range(0, 3);
        Debug.Log(randomIndex);
        switch (randomIndex)
        {
            case 0:
                bloodAnimator.Play("Blood1");
                return;
            case 1:
                bloodAnimator.Play("Blood2");
                return;
            case 2:
                bloodAnimator.Play("Blood3");
                return;
        }
    }
    public void ShowHitFX(Vector3 position)
    {
        hitAnimator.transform.position = position;
        hitAnimator.gameObject.SetActive(true);
        int randomIndex = Random.Range(0, 2);
        if (randomIndex == 1)
            hitAnimator.Play("Hit1");
        else
            hitAnimator.Play("Hit2");
    }
}
