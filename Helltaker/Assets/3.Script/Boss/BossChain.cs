using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChain : MonoBehaviour
{
    public int chainLife;
    public void KickChain()
    {
        chainLife -= 1;
        this.gameObject.GetComponentInParent<BossMode>().UpdateChainHealth();
        if (chainLife <= 0)
            gameObject.SetActive(false);
    }
}
