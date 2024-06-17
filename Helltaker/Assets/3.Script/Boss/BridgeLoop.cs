using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeLoop : MonoBehaviour
{
    private float height;

    private void Start()
        => height = transform.GetComponent<BoxCollider2D>().size.y;


    private void Update()
    {
        if (transform.position.y >= height)
        {
            //offset -> vector2 ��ǥ�� ���� ���� transform�� ����
            Vector2 offset = new Vector2(0, height * 3);
            transform.position = (Vector2)transform.position - offset;
        }
    }
}
