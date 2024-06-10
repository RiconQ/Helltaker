using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 1f;
    private Coroutine moveCoroutine;
    private bool isMoving = false;
    private Animator animator;
    private SpriteRenderer renderer;

    private void Awake()
    {
        renderer = transform.GetComponent<SpriteRenderer>();
        animator = transform.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!isMoving)
            InputMove();
    }

    private void InputMove()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            Move(0, 1);
        else if (Input.GetKey(KeyCode.DownArrow))
            Move(0, -1);     
        else if (Input.GetKey(KeyCode.LeftArrow))
            Move(-1, 0);     
        else if (Input.GetKey(KeyCode.RightArrow))
            Move(1, 0);
    }

    private void Move(int x, int y)
    {
        isMoving = true;
        animator.SetBool("isMoving", true);
        //transform.position += new Vector3(x, y, 0);
        if (x == -1)
            renderer.flipX = true;
        else if (x == 1)
            renderer.flipX = false;

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
        yield return new WaitForSeconds(0.1f);
        isMoving = false;
        animator.SetBool("isMoving", false);
    }
}
