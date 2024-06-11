using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 10f;
    private bool isMoving = false;
    private Animator animator;
    private SpriteRenderer renderer;

    [SerializeField] private bool hasKey = false;

    private void Awake()
    {
        renderer = transform.GetComponent<SpriteRenderer>();
        animator = transform.GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isMoving)
        {
            //Debug.Log(isMoving);
            InputMove();
        }
        //Debug.Log(isMoving);
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
        else return;
        // 무브가 끝나고 현재 위치에 가시가 있는지 확인 
        //CheckSpike();
    }

    private void Move(int x, int y)
    {
        if (x == -1)
            renderer.flipX = true;
        else if (x == 1)
            renderer.flipX = false;

        isMoving = true;

        Collider2D collider = GetRay(x, y);
        if (collider != null)
            if (CheckObstacle(collider, x, y))
            {
                CheckSpike();
                return;
            }

        GameManager.instance.UseTurn(1);

        animator.SetBool("isMoving", true);
        //transform.position += new Vector3(x, y, 0);

        StartCoroutine(Move_co(new Vector3(x, y, 0)));
    }
    private IEnumerator Move_co(Vector3 direction)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + direction;
        float elapsedTime = 0;

        while (elapsedTime < 1.0f)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }

        transform.position = targetPosition;
        //yield return new WaitForSeconds(0.2f);
        CheckSpike();
        isMoving = false;
        animator.SetBool("isMoving", false);
    }

    private Collider2D GetRay(float x, float y, float dist = 0.3f)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(x, y, 0), new Vector3(x, y, 0), dist, LayerMask.GetMask("Obstacle"));
        //Debug.DrawRay(transform.position + new Vector3(x, y, 0), new Vector3(x, y, 0) * dist, Color.red, 3f);
        if (hit.collider != null)
        {
            //Debug.Log(hit.collider.name);
            return hit.collider;
        }
        return null;
    }

    private bool CheckObstacle(Collider2D collider, int x, int y)
    {

        switch (collider.gameObject.tag)
        {
            // 박스, 해골
            case "Kickable":
                GameManager.instance.UseTurn(1);
                animator.SetTrigger("Kick");
                collider.gameObject.GetComponent<Kickable>().Kick(x, y);

                isMoving = false;
                return true;
            // 열쇠, 자물쇠
            case "Key":
                this.hasKey = true;
                Destroy(collider.gameObject);
                return false;
            case "Lock":
                if (!hasKey)
                {
                    GameManager.instance.UseTurn(1);
                    animator.SetTrigger("Kick");
                    isMoving = false;
                    return true;
                }
                else
                {
                    this.hasKey = true;
                    Destroy(collider.gameObject);
                    return false;
                }
            // 송곳일경우
            //case "Spike":
            //    GameManager.instance.UseTurn(1);
            //    Debug.Log("Spike");
            //    return false;
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
                GameManager.instance.UseTurn(1);
                //Debug.Log("Spike.");
            }
        }
    }
}
