using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }

        else
        {
            Debug.Log("�̹� �� ���ӿ��� ���� �Ŵ����� �����մϴ�.");
            Destroy(gameObject);
        }
    }

    [SerializeField] private int remainTurn;
    
    public int RemainTurn
    {
        get { return remainTurn; }
    }


    public void UseTurn(int turn)
    {
        // ���� �� 0���� �ൿ�� ���
        if (remainTurn == 0)
        {
            Debug.Log("���");
            return;
        }
        if (remainTurn - turn <= 0)
            remainTurn = 0;
        else if (remainTurn - turn > 0)
            remainTurn -= turn;
        Debug.Log($"Remain Turn : {remainTurn}");
    }
}
