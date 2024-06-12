using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [SerializeField] private Text turnText;
    private GameObject playerObj;
    [SerializeField] private GameObject deathAnim;
    [SerializeField] private GameObject fadeOutAnim;

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

    private void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        UpdateTurnText();
    }


    public bool UseTurn(int turn)
    {
        // ���� �� 0���� �ൿ�� ���
        if (remainTurn == 0)
        {
            OnDie();
            return false;
        }
        if (remainTurn - turn <= 0)
            remainTurn = 0;
        else if (remainTurn - turn > 0)
            remainTurn -= turn;
        UpdateTurnText();
        return true;
        Debug.Log($"Remain Turn : {remainTurn}");
    }

    private void UpdateTurnText()
    {
        turnText.text = remainTurn.ToString();
        if (remainTurn == 0) turnText.text = "X";
    }

    private void OnDie()
    {
        deathAnim.transform.position = playerObj.transform.position;
        deathAnim.SetActive(true);
    }

    public void RestartLevel()
    {
        LevelManager.instance.SetNextLevelName(SceneManager.GetActiveScene().name);
        fadeOutAnim.SetActive(true);
    }
}
