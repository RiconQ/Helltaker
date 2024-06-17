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
            Cursor.visible = false;
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
    public bool usingTurn = true;


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
        if (!usingTurn) return true;
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
        try
        {
            turnText.text = remainTurn.ToString();
            if (remainTurn == 0) turnText.text = "X";
        }
        catch
        { }
    }

    public void OnDie()
    {
        Debug.Log("Die");
        CameraShakeManager.instance.shakeTime = 1.0f;
        CameraShakeManager.instance.Shake();
        deathAnim.transform.position = playerObj.transform.position;
        deathAnim.SetActive(true);
    }

    public void RestartLevel()
    {
        LevelManager.instance.SetNextLevelName(SceneManager.GetActiveScene().name);
        fadeOutAnim.SetActive(true);
    }

    public void NextLevel(string levelName)
    {
        LevelManager.instance.SetNextLevelName(levelName);
        fadeOutAnim.SetActive(true);
    }
}
