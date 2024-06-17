using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlayer : MonoBehaviour
{
    [SerializeField] private GameObject[] sinPyre;
    [SerializeField] private int index = 0;
    [SerializeField] private Sprite fireOff;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("BossChain"))
        {
            if (index == 3)
                GameManager.instance.OnDie();
            sinPyre[index].GetComponent<SpriteRenderer>().sprite = fireOff;
            sinPyre[index].GetComponentInChildren<Animator>().gameObject.SetActive(false);
            index += 1;
        }
    }
}
