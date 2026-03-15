using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] int scoreValue = 100;
    [SerializeField] GameObject hitParticles;

    public int health = 1;
    private AudioSource hitSound;

    private void Start()
    {
        hitSound = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            hitSound.Play();

            Instantiate(hitParticles, transform.position, Quaternion.identity);

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
            GameManager.Instance.AddScore(scoreValue);
            Die();
        }
        else
        {
            Ball ball = FindFirstObjectByType<Ball>();
            if (ball != null)
            {

            }
        }
    }

    private void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 0.1f);
    }
}
