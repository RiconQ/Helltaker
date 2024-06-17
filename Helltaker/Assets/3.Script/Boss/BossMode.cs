using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class BossMode : MonoBehaviour
{
    [SerializeField] private GameObject[] bossChain;
    [SerializeField] private GameObject chainUI;
    [SerializeField] private Slider healthBar;

    [Header("Stop Scroll")]
    [SerializeField] private GameObject bgChainLeft;
    [SerializeField] private GameObject bgChainRight;

    [SerializeField] private GameObject bridge;

    [SerializeField] private GameObject sinPistonLeft;
    [SerializeField] private GameObject sinPistonRight;

    [SerializeField] private GameObject spikeUp;
    [SerializeField] private GameObject spikeDown;

    [SerializeField] private GameObject player;

    [SerializeField] private GameObject bgGear;

    [Header("Judgement")]
    [SerializeField] private GameObject judgementObj;

    private void Start()
    {
        healthBar.gameObject.SetActive(true);
        healthBar.maxValue = GetChainHealth();
        healthBar.value = healthBar.maxValue;
        healthBar.gameObject.SetActive(false);
    }
    public void ShowChain()
    {
        healthBar.gameObject.SetActive(true);
        foreach (var item in bossChain)
            item.SetActive(true);
    }
    public int GetChainHealth()
    {
        int tmp = 0;
        foreach (var item in bossChain)
            tmp += item.GetComponent<BossChain>().chainLife;
        return tmp;
    }
    public int GetChainMaxHealth()
    {
        int tmp = 0;
        foreach (var item in bossChain)
            tmp += item.GetComponent<BossChain>().chainLife;
        //Debug.Log(tmp);
        return tmp;
    }

    public void UpdateChainHealth()
    {
        healthBar.value = GetChainMaxHealth();
        if (healthBar.value <= 0)
        {
            chainUI.SetActive(false);
            player.GetComponent<PlayerControl>().SetIsMoving(false);
            // 저지먼트 대화 시작.
            judgementObj.SetActive(true);
            this.GetComponent<PlayableDirector>().enabled = false;
        }
    }

    public void StopScroll()
    {
        CameraShakeManager.instance.Shake();

        foreach (var item in bgChainLeft.GetComponentsInChildren<Scroll>())
            item.SetIsMoving(false);
        foreach (var item in bgChainRight.GetComponentsInChildren<Scroll>())
            item.SetIsMoving(false);
        foreach (var item in bridge.GetComponentsInChildren<Scroll>())
            item.SetIsMoving(false);
        foreach (var item in sinPistonLeft.GetComponentsInChildren<Animator>())
            item.enabled = false;
        foreach (var item in sinPistonRight.GetComponentsInChildren<Animator>())
            item.enabled = false;
        foreach (var item in spikeUp.GetComponentsInChildren<Scroll>())
        {
            item.SetIsMoving(false);
            item.GetComponent<SpikeLoopUp>().StopMoving();
        }
        foreach (var item in spikeDown.GetComponentsInChildren<Scroll>())
        {
            item.SetIsMoving(false);
            item.GetComponent<SpikeLoop>().StopMoving();
        }

        player.GetComponent<Scroll>().SetIsMoving(false);

        foreach (var item in bgGear.GetComponentsInChildren<Animator>())
            item.Play("BG_Gear_Idle");
    }
}
