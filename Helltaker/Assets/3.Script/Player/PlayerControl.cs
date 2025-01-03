using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private bool hasKey = false;
    private bool isMoving = false;
    [SerializeField] private bool hasAdvice = true;
    [SerializeField] private bool isBoss = false;
    [SerializeField] private PauseMenu pauseMenu;

    public AnimController playerAnimator;
    //private SpriteRenderer renderer;

    //[SerializeField] private Animator runAnimator;
    //private Animator animator;


    private void Awake()
    {
        playerAnimator = GetComponent<AnimController>();
        TryGetComponent(out pauseMenu);
        //renderer = transform.GetComponent<SpriteRenderer>();
        //animator = transform.GetComponent<Animator>();
    }

    private void Update()
    {
        if (!DialogueManager.instance.isDialogue)
        {
            if (!DialogueManager.instance.isMainMenu)
            {

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    //게임이 멈춰있다면
                    if (pauseMenu.gamePause)
                    {
                        pauseMenu.TogglePause(false);
                        pauseMenu.gamePause = false;
                    }
                    //게임이 진행중이라면
                    else
                    {
                        pauseMenu.TogglePause(true);
                        pauseMenu.gamePause = true;
                    }
                }
            }
            if (!isMoving && !pauseMenu.gamePause)
            {
                //Debug.Log(isMoving);
                InputMove();
            }

        }
        //Debug.Log(isMoving);
    }

    public void SetIsMoving(bool value)
    {
        isMoving = value;
    }

    private void InputMove()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            Move(0, 1);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            Move(0, -1);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            Move(-1, 0);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            Move(1, 0);
        // 다시 시작
        else if (Input.GetKey(KeyCode.R))
            GameManager.instance.RestartLevel();
        // 인생 조언
        else if (Input.GetKey(KeyCode.L) && hasAdvice)
            DialogueManager.instance.GetInteractionEvent(
                GetComponent<InteractionEvent>());
        else return;
        // 무브가 끝나고 현재 위치에 가시가 있는지 확인 
        //CheckSpike();
    }

    private void Move(int x, int y)
    {
        if (x == -1)
            playerAnimator.FlipX(true);
        else if (x == 1)
            playerAnimator.FlipX(false);

        isMoving = true;
        Collider2D collider;
        if (!isBoss)
            collider = GetRay(x, y);
        else
            collider = GetBossRay(x, y);

        if (collider != null)
            if (CheckObstacle(collider, x, y))
            {
                if (collider.gameObject.CompareTag("BossChain"))
                {
                    collider.gameObject.GetComponent<BossChain>().KickChain();
                    Debug.Log("Boss Chain");
                }
                if (!collider.gameObject.CompareTag("Wall"))
                {
                    TryGetManager();
                    CheckSpike();
                }

                return;
            }

        TryGetManager();
        if (!GameManager.instance.UseTurn(1)) return;

        playerAnimator.SetBoolPlayer("isMoving", true);
        //transform.position += new Vector3(x, y, 0);

        StartCoroutine(Move_co(new Vector3(x, y, 0)));
    }
    private IEnumerator Move_co(Vector3 direction)
    {
        // 현재 위치에 이동하는 fx 소환
        playerAnimator.ShowMoveFX(transform.position);


        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + direction;
        float elapsedTime = 0;

        while (elapsedTime < 1.0f)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            elapsedTime += 0.01f * moveSpeed;
            yield return new WaitForSeconds(0.01f);
        }
        Vector3 roundPosition = new Vector3(Mathf.Round(targetPosition.x * 2) / 2.0f, Mathf.Round(targetPosition.y * 2) / 2.0f, 0);
        transform.position = roundPosition;
        //transform.position = targetPosition;
        //yield return new WaitForSeconds(0.2f);
        CheckSpike();
        isMoving = false;
        playerAnimator.SetBoolPlayer("isMoving", false);
    }

    private Collider2D GetRay(float x, float y, float dist = 0.3f)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(x, y, 0), new Vector3(x, y, 0), dist, LayerMask.GetMask("Obstacle"));
        //Debug.DrawRay(transform.position + new Vector3(x, y, 0), new Vector3(x, y, 0) * dist, Color.red, 3f);
        if (hit.collider != null)
        {
            // Debug.Log(hit.collider.name);
            return hit.collider;
        }
        return null;
    }
    private Collider2D GetBossRay(float x, float y, float dist = 0.3f)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(x, y, 0), new Vector3(x, y, 0), dist, LayerMask.GetMask("BossObstacle"));
        //Debug.DrawRay(transform.position + new Vector3(x, y, 0), new Vector3(x, y, 0) * dist, Color.red, 3f);
        if (hit.collider != null)
        {
            //Debug.Log(hit.collider.name);
            return hit.collider;
        }
        return GetRay(x, y);
    }


    private bool CheckObstacle(Collider2D collider, int x, int y)
    {

        switch (collider.gameObject.tag)
        {
            // 박스, 해골
            case "Kickable":
                if (!GameManager.instance.UseTurn(1)) return false;
                playerAnimator.SetTriggerPlayer("Kick");
                playerAnimator.ShowHitFX(transform.position + new Vector3(x, y, 0));
                collider.gameObject.GetComponent<Kickable>().Kick(x, y);
                //TryGetManager();
                isMoving = false;
                return true;
            // 열쇠, 자물쇠
            case "Key":
                Debug.Log("Key");
                //TryGetManager();
                this.hasKey = true;
                collider.gameObject.GetComponent<Animator>().SetTrigger("GetKey");
                //Destroy(collider.gameObject);
                return false;
            case "Lock":
                if (!hasKey)
                {
                    Debug.Log("not has key");
                    //TryGetManager();
                    if (!GameManager.instance.UseTurn(1)) return false;
                    playerAnimator.SetTriggerPlayer("Kick");
                    isMoving = false;
                    return true;
                }
                else
                {
                    Debug.Log("has key");
                    //TryGetManager();
                    this.hasKey = true;
                    collider.gameObject.GetComponent<Animator>().SetTrigger("GetKey");
                    return false;
                }
            // 송곳일경우
            //case "Spike":
            //    GameManager.instance.UseTurn(1);
            //    Debug.Log("Spike");
            //    return false;
            case "BossChain":
                playerAnimator.SetTriggerPlayer("Kick");
                playerAnimator.ShowHitFX(transform.position + new Vector3(x, y, 0));
                isMoving = false;
                return true;
            default:
                isMoving = false;
                return true;
        }
    }

    private void CheckSpike()
    {
        Vector2 currentPosition = transform.position;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(currentPosition, 0.1f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Spike"))
            {

                if (collider.gameObject.GetComponent<Spike>().GetIsSpike())
                {
                    playerAnimator.ShowBloodFX(currentPosition);
                    CameraShakeManager.instance.Shake();
                    GameManager.instance.UseTurn(1);
                }
                //Debug.Log("Spike.");
            }
        }
    }

    private void TryGetManager()
    {
        try
        {
            SpikeManager.instance.ToggleSpike();
        }
        catch
        {
            //Debug.Log("Not Found Spike Manager");
        }

        try
        {
            SkeletonManager.instance.CheckSkeletonSpike();
        }
        catch
        {
            //Debug.Log("Not Found Skeleton Manager");
        }
    }
    public void HomePlayerMove()
    {
        Move(0, 1);
    }
}
