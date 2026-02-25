using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class EnemyGroupController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveDownAmount = 0.5f;
    [SerializeField] private float leftBoundary = -8f;
    [SerializeField] private float rightBoundary = 8f;
    [SerializeField] private float changeDirectionDelay = 0.25f;
    [SerializeField] private float initialDelay = 0.15f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootInterval = 2f;

    private Vector2 moveDirection = Vector2.right;
    private float shootTimer;

    void Update()
    {
        if (GameManager.Instance.currentState != GameManager.GameState.Playing)
        {
            return;
        }

        if (transform.childCount == 0)
        {
            Debug.Log("All enemies defeated! You win!");
        }

        if (bulletPrefab == null)
        {
            return;
        }

        shootTimer += Time.deltaTime;

        if (shootTimer >= shootInterval)
        {
            Shoot();
            shootTimer = 0f;
        }

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        CheckEdges();
    }

    private void CheckEdges()
    {
        float leftEdge = float.MaxValue;
        float rightEdge = float.MinValue;

        foreach (Transform enemy in transform)
        {
            if (enemy == null) continue;

            float x = enemy.position.x;

            if (x < leftEdge) leftEdge = x;
            if (x > rightEdge) rightEdge = x;
        }

        if (rightEdge >= rightBoundary && moveDirection.x > 0)
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
        if (moveDirection.x > 0)
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
        yield return new WaitForSeconds(initialDelay);
        moveDirection = direction;
    }

    private void Shoot()
    {
        List<Transform> shooters = GetBottomEnemies();

        if (shooters.Count == 0) return;

        Transform shooter = shooters[Random.Range(0, shooters.Count)];
        Instantiate(bulletPrefab, shooter.position, Quaternion.identity);
    }

    List<Transform> GetBottomEnemies()
    {
        Dictionary<int, Transform> bottomEnemies = new Dictionary<int, Transform>();

        foreach(Transform enemy in transform)
        {
            int column = Mathf.RoundToInt(enemy.position.x * 10f);

            if(!bottomEnemies.ContainsKey(column) || enemy.position.y < bottomEnemies[column].position.y)
            {
                bottomEnemies[column] = enemy;
            }
        }

        return new List<Transform>(bottomEnemies.Values);
    }
}
