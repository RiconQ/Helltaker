using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kickable : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 1f;
    public void Kick(int x, int y)
    {
        // ���� ���⿡ ��ֹ��� �ִ��� �˻�
        // ��ֹ��� �ִٸ� �̵� �Ұ�, �� �Һ� (Player����)
        // ��ֹ��� ���ٸ� ���� �������� �̵�
        Move(x, y);
    }

    public void Move(int x, int y)
    {
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
    }
}
