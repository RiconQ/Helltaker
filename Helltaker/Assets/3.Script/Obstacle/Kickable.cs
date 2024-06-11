using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kickable : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 10f;
    public void Kick(int x, int y)
    {
        // 진행 방향에 장애물이 있는지 검사
        Collider2D collider = GetRay(x, y);
        if (collider != null)
        {
            // 장애물이 있다면 이동 불가, 턴 소비 (Player에서)
            if (collider.gameObject.CompareTag("Kickable"))
            {

                return;
            }
            Destroy(gameObject);
            // 장애물이 있지만 장애물이 kickable이 아니면 부서짐(Destroy)
            return;
        }
        // 장애물이 없다면 진행 방향으로 이동
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

    private Collider2D GetRay(int x, int y)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (new Vector3(x, y, 0) * 0.6f), new Vector3(x, y, 0), 0.2f, LayerMask.GetMask("Obstacle")); ;
        Debug.DrawRay(transform.position, new Vector3(x, y, 0), Color.red, 3f);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);
            return hit.collider;
        }
        return null;
    }
}
