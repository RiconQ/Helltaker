using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeLoopUp : MonoBehaviour
{

    private float height;
    [SerializeField] private float margin;
    private bool playing = false;

    [SerializeField] private Vector3 stopPosition;

    private void Start()
        => height = transform.GetComponent<BoxCollider2D>().size.y;


    private void Update()
    {
        if (transform.position.y >= margin)
        {
            //offset -> vector2 좌표를 만들어서 현재 transform에 더함
            Vector2 offset = new Vector2(0, height * 2);
            transform.position = (Vector2)transform.position - offset;
            this.gameObject.SetActive(false);
            this.gameObject.SetActive(true);
            //GetComponentInChildren<Animator>().Play("spikeLineUp");
        }
    }

    public void StopMoving()
    {
        this.transform.position = stopPosition;
    }
}
