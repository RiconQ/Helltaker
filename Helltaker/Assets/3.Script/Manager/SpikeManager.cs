using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeManager : MonoBehaviour
{
    public static SpikeManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }

        else
        {
            Debug.Log("이미 이 게임에는 Spike 매니저가 존재합니다.");
            Destroy(gameObject);
        }
    }

    [SerializeField] Spike[] movingSpike;

    public void ToggleSpike()
    {
        for(int i = 0; i < movingSpike.Length; i++)
        {
            movingSpike[i].GetComponent<Animator>().SetBool("SpikeOut", !movingSpike[i].GetIsSpike());
            movingSpike[i].ToggleSpike();
        }
    }
}
