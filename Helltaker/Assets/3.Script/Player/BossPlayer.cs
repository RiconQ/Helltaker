using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlayer : MonoBehaviour
{
    [SerializeField] private GameObject[] sinPyre;
    [SerializeField] private int index = 0;
    [SerializeField] private Sprite fireOff;
    public bool isCheat = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("BossChain"))
        {
            if (isCheat) return;
            if (index == 3)
                GameManager.instance.OnDie();

            else
            {
                CameraShakeManager.instance.Shake();
                this.GetComponent<PlayerControl>().playerAnimator.ShowBloodFX(transform.position);
                sinPyre[index].GetComponent<SpriteRenderer>().sprite = fireOff;
                sinPyre[index].GetComponentInChildren<Animator>().gameObject.SetActive(false);
                index += 1;
            }
        }
    }
}
