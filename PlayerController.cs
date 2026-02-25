using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float invulnerabilityDuration = 0.75f;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isInvulnerable = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(GameManager.Instance.currentState != GameManager.GameState.Playing)
        {
            return;
        }

        moveInput = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet") && !isInvulnerable)
        {
            StartCoroutine(HitRoutine());

            Ball ball = FindFirstObjectByType<Ball>();

            if(ball != null)
            {
                ball.ResetBall();
            }

            GameManager.Instance.LoseLife();
            Destroy(collision.gameObject);
        }
    }

    IEnumerator HitRoutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }
}
