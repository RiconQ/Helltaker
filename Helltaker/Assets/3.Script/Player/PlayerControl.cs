using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Ű���� �Է����� �����¿� �̵�
    // Ű���� �� �� �Է����� �� �� �̵�
    // ��� ������ ������ ���� ���������� �̵� -> ������ ���� -> �ڷ�ƾ?
    // ī���� X���ķ� ����Ű �Է��� ������ ���

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        // Ű���� �Է½� Move �Լ� ȣ��
        // ���Ŀ� ���� �ʿ�
        InputMove();
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
        transform.position += new Vector3(x, y, 0);
    }
}
