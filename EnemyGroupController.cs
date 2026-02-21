using System.Collections;
using UnityEngine;

public class EnemyGroupController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveDownAmount = 0.5f;
    [SerializeField] private float leftBoundary = -8f;
    [SerializeField] private float rightBoundary = 8f;
    [SerializeField] private float changeDirectionDelay = 0.25f;
    [SerializeField] private float initialDelay = 0.15f;

    private Vector2 moveDirection = Vector2.right;

    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        CheckEdges();
    }

    private void CheckEdges()
    {
        float leftEdge = float.MaxValue;
        float rightEdge = float.MinValue;

        foreach (Transform enemy in transform)
        {
            if(enemy == null) continue;

            float x = enemy.position.x;

            if (x < leftEdge) leftEdge = x;
            if (x > rightEdge) rightEdge = x;
        }

        if(rightEdge >= rightBoundary && moveDirection.x > 0)
        {
            MoveDown();
        }

        if (leftEdge <= leftBoundary && moveDirection.x < 0)
        {
            MoveDown();
        }
    }

    private void MoveDown()
    {
        if(moveDirection.x > 0)
        {
            moveDirection = Vector2.zero;
            StartCoroutine(ChangeDirection(Vector2.left));
        }
        else
        {
            moveDirection = Vector2.zero;
            StartCoroutine(ChangeDirection(Vector2.right));
        }
    }

    private IEnumerator ChangeDirection(Vector2 direction)
    {
        yield return new WaitForSeconds(initialDelay);
        moveDirection = new Vector2(0f, -moveDownAmount);
        yield return new WaitForSeconds(changeDirectionDelay);
        moveDirection = Vector2.zero;
        //transform.position += new Vector3(moveDirection.x * 0.2f, -moveDownAmount, 0f);
        yield return new WaitForSeconds(initialDelay);
        moveDirection = direction;
    }
}
