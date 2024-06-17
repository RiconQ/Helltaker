using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private bool isMoving;
    void FixedUpdate()
    {
        if (isMoving)
            transform.Translate(Vector3.up * Time.fixedDeltaTime * _speed);

    }
    public void SetIsMoving(bool value)
    {
        isMoving = value;
        try
        {
            GetComponentInChildren<Animator>().Play("Idle");
        }
        catch { }
    }
}
