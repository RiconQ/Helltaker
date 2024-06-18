using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartParticleManager : MonoBehaviour
{
    public static HeartParticleManager instance = null;

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
            Debug.Log("Already have HeartParticleManager");
            Destroy(gameObject);
        }
    }

    public GameObject[] npcs;
    private ParticleSystem heartParticle;

    private void Start()
    {
        TryGetComponent(out heartParticle);
    }

    public void PlayHeart()
    {
        foreach (var item in npcs)
        {
            transform.position = item.transform.position;
            heartParticle.Play();
        }
    }

    public void PlayVictoryAnim()
    {
        foreach(var item in npcs)
        {
            item.GetComponent<Animator>().Play("Victory");
        }
    }
}
