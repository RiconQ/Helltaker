using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneParticleManager : MonoBehaviour
{
    public static BoneParticleManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            Cursor.visible = false;
            instance = this;
            //DontDestroyOnLoad(this);
        }

        else
        {
            Debug.Log("Already have BoneParticleManager");
            Destroy(gameObject);
        }
    }

    private ParticleSystem boneParticle;
    private void Start()
    {
        TryGetComponent(out boneParticle);    
    }

    public void PlayBoneParticle(Vector3 position)
    {
        this.transform.position = position;
        boneParticle.Play();
    }
}
