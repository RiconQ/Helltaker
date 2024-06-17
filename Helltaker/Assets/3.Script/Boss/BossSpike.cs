using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpike : MonoBehaviour
{
    private void Start()
    {
        //GetComponent<Animator>().Play("spikeLineStart");    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            GameManager.instance.OnDie();
    }
}
