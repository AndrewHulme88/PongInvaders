using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health = 1;
    public float moveSpeed = 2f;

    private Vector2 moveDirection = Vector2.right;

    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            health--;
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            moveDirection = -moveDirection;
        }
    }
}
