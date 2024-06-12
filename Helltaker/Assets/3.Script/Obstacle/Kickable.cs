using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kickable : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private bool breakable = false;

    //breakable true��� skelAnimator, renderer �Ҵ�
    private Animator skelAnimator;
    private SpriteRenderer renderer;

    private void Awake()
    {
        if (breakable)
        {
            TryGetComponent(out skelAnimator);
            TryGetComponent(out renderer);
        }
    }

    public void Kick(int x, int y)
    {
        // ���� ���⿡ ��ֹ��� �ִ��� �˻�
        Collider2D collider = GetRay(x, y);
        if (collider != null)
        {
            // breakable�� true��� �ش� ������Ʈ �μ���(Destroy)
            if (breakable)
            {
                Destroy(gameObject);
                return;
            }
            // ��ֹ��� �ִٸ� �̵� �Ұ�, �� �Һ� (Player����)
            if (collider.gameObject.CompareTag("Kickable") || collider.gameObject.CompareTag("Lock") || collider.gameObject.CompareTag("Wall"))
            {
                return;
            }

        }
        // ��ֹ��� ���ٸ� ���� �������� �̵�
        Move(x, y);
    }

    public void Move(int x, int y)
    {
        if (breakable)
        {
            if (x == -1)
                renderer.flipX = false;
            else if (x == 1)
                renderer.flipX = true;
        }

        if (breakable)
            skelAnimator.SetTrigger("Kick");

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
        if (breakable)
            CheckSpike();
        //yield return new WaitForSeconds(0.2f);
    }

    private Collider2D GetRay(int x, int y)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(x, y, 0), new Vector3(x, y, 0), 0.2f, LayerMask.GetMask("Obstacle")); ;
        //Debug.DrawRay(transform.position, new Vector3(x, y, 0), Color.red, 3f);
        if (hit.collider != null)
        {
            //Debug.Log(hit.collider.name);
            return hit.collider;
        }
        return null;
    }
    public void CheckSpike()
    {
        Vector2 currentPosition = transform.position;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(currentPosition, 0.1f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Spike") && collider.gameObject.GetComponent<Spike>().GetIsSpike())
            {
                Destroy(gameObject);
                //Debug.Log("Spike.");
            }
        }
    }
}
