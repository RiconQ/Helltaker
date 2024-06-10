using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // 키보드 입력으로 상하좌우 이동
    // 키보드 한 번 입력으로 한 번 이동
    // 계속 누르고 있으면 벽을 만날때까지 이동 -> 일정한 간격 -> 코루틴?
    // 카운터 X이후로 방향키 입력이 있을시 사망

    [SerializeField] private float moveDuration = 1f;
    private Coroutine moveCoroutine;
    private bool isMoving = false;
    private Animator animator;

    private void Awake()
    {
        animator = transform.GetComponent<Animator>();
    }

    private void Update()
    {
        //float x = Input.GetAxisRaw("Horizontal");
        //float y = Input.GetAxisRaw("Vertical");
        // 키보드 입력시 Move 함수 호출
        // 추후에 수정 필요

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
        Debug.Log($"move : {x}, {y}");
        animator.SetFloat("Horizontal", x);
        animator.SetFloat("Vertical", y);
        isMoving = true;
        moveCoroutine = StartCoroutine(Move_co(new Vector3(x, y, 0)));

    }

    private IEnumerator Move_co(Vector3 direction)
    {
        Vector3 targetPosition = transform.position + direction;
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", 0);
    }
}
