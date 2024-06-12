using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonManager : MonoBehaviour
{
    public static SkeletonManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }

        else
        {
            Debug.Log("이미 이 게임에는 SkeletonManager 존재합니다.");
            Destroy(gameObject);
        }
    }

    [SerializeField] Kickable[] skeleton;

    public void CheckSkeletonSpike()
    {
        for (int i = 0; i < skeleton.Length; i++)
        {
            skeleton[i].CheckSpike();
        }
    }
}
