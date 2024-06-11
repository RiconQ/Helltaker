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
            Debug.Log("이미 이 게임에는 게임 매니저가 존재합니다.");
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
        // 남은 턴 0에서 행동시 사망
        if (remainTurn == 0)
        {
            Debug.Log("사망");
            return;
        }
        if (remainTurn - turn <= 0)
            remainTurn = 0;
        else if (remainTurn - turn > 0)
            remainTurn -= turn;
        Debug.Log($"Remain Turn : {remainTurn}");
    }
}
