using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucyCamera : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject death;
    private void FixedUpdate()
    {
        if (!death.activeSelf)
            this.transform.position = new Vector3(0, player.transform.position.y + 2.5f, -10);
    }
}
