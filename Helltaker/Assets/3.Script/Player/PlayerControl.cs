using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Ű���� �Է����� �����¿� �̵�
    // Ű���� �� �� �Է����� �� �� �̵�
    // ��� ������ ������ ���� ���������� �̵� -> ������ ���� -> �ڷ�ƾ?
    // ī���� X���ķ� ����Ű �Է��� ������ ���

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
        // Ű���� �Է½� Move �Լ� ȣ��
        // ���Ŀ� ���� �ʿ�

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
