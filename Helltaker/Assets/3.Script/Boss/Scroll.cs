using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private bool isMoving;
    [SerializeField] private Vector3 direction = Vector3.up;
    void FixedUpdate()
    {
        if (isMoving)
            transform.Translate(direction * Time.fixedDeltaTime * _speed);

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
