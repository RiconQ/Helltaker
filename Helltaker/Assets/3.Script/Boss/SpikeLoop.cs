using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeLoop : MonoBehaviour
{
    private float height;
    [SerializeField] private float margin;
    private bool playing = false;
    [SerializeField] private bool isUp;

    [SerializeField] private Vector3 stopPosition;

    private void Start()
        => height = transform.GetComponent<BoxCollider2D>().size.y;


    private void Update()
    {
        if (transform.position.y >= margin - 1 && !playing)
        {
            playing = true;

            GetComponentInChildren<Animator>().Play("spikeLine");

        }
        if (transform.position.y >= margin)
        {
            //offset -> vector2 좌표를 만들어서 현재 transform에 더함
            Vector2 offset = new Vector2(0, height * 2);
            transform.position = (Vector2)transform.position - offset;
            playing = false;
            GetComponentInChildren<Animator>().Play("Idle");
        }
    }
    public void StopMoving()
    {
        this.transform.position = stopPosition;
    }
}
