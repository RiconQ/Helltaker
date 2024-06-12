using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private bool isSpikeOn = true;

    public void ToggleSpike()
    {
        isSpikeOn = !isSpikeOn;
    }

    public bool GetIsSpike()
    {
        return isSpikeOn;
    }
}
