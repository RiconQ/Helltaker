using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // 키보드 입력으로 상하좌우 이동
    // 키보드 한 번 입력으로 한 번 이동
    // 계속 누르고 있으면 벽을 만날때까지 이동 -> 일정한 간격 -> 코루틴?
    // 카운터 X이후로 방향키 입력이 있을시 사망

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        // 키보드 입력시 Move 함수 호출
        // 추후에 수정 필요
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
