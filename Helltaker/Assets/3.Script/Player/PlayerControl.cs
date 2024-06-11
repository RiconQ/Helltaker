using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 1f;
    private bool isMoving = false;
    private Animator animator;
    private SpriteRenderer renderer;

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
        {
            //StartCoroutine(Move_co(new Vector3(0, 0, 0)));
            if(collider.gameObject.CompareTag("Kickable"))
            {
                //킥 애니메이션 출력
                // collider 킥 함수 실행
                animator.SetTrigger("Kick");
                collider.gameObject.GetComponent<Kickable>().Kick(x, y);

                isMoving = false;
                return;
            }
            //Debug.Log(collider.gameObject.tag);
            isMoving = false;
            return;
        }

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
        isMoving = false;
        animator.SetBool("isMoving", false);
    }

    private Collider2D GetRay(int x, int y)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(x, y, 0), 1, LayerMask.GetMask("Obstacle"));
        Debug.DrawRay(transform.position, new Vector3(x, y, 0), Color.red, 3f);
        if(hit.collider != null)
        {
            //Debug.Log(hit.collider.name);
            return hit.collider;
        }
        return null;
    }
}
