using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            health--;
            if (health <= 0)
            {
                TakeDamage(1);
            }
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 0.1f);
    }
}
