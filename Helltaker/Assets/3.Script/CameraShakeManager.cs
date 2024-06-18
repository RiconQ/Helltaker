using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }

        else
        {
            Debug.Log("�̹� �� ���ӿ��� CameraShakeManager �����մϴ�.");
            Destroy(gameObject);
        }
    }

    public float shakeAmount = 0.1f;
    public float shakeTime = 0.2f;

    public void Shake()
    {
        StartCoroutine(Shake_co());
    }

    private IEnumerator Shake_co()
    {
        float timer = 0;
        while (timer <= shakeTime)
        {
            Camera.main.transform.position = 
                new Vector3(Random.RandomRange(-1f, 1f) * shakeAmount, Random.RandomRange(-1f, 1f) * shakeAmount, -10) ;
           // Debug.Log(Camera.main.transform.position);
            timer += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        Camera.main.transform.position = new Vector3(0f, 0f, -10.0f);
    }
}
