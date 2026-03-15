using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Transform playerPaddle;
    [SerializeField] Vector3 ballOffset;
    [SerializeField] float minYVelocity = 0.25f;
    [SerializeField] GameObject hitParticles;

    public float speed = 5f;
    
    private Rigidbody2D rb;
    private bool isLaunched = false;
    private AudioSource hitSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        hitSound = GetComponent<AudioSource>();

        ResetBall();
    }

    private void FixedUpdate()
    {
        if(GameManager.Instance.currentState != GameManager.GameState.Playing)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (rb.linearVelocity != Vector2.zero)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * speed;
        }
    }

    void Update()
    {
        if (!isLaunched)
        {
            transform.position = playerPaddle.position + ballOffset;
        }
    }

    public void LaunchBall()
    {
        if (isLaunched)
        {
            return;
        }

        isLaunched = true;
        Vector2 direction = new Vector2(Random.Range(-0.5f, 0.5f), 1).normalized;
        rb.linearVelocity = direction * speed;
    }

    public void ResetBall()
    {
        isLaunched = false;
        rb.linearVelocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bottom"))
        {
            collision.gameObject.GetComponent<AudioSource>().Play();
            FindFirstObjectByType<CameraController>().ShakeCamera();
            GameManager.Instance.LoseLife();
            ResetBall();
        }

        if(collision.gameObject.CompareTag("Player"))
        {
            if(isLaunched)
            {
                Instantiate(hitParticles, transform.position, Quaternion.identity);
            }

            float hitPoint = transform.position.x - collision.transform.position.x;
            float paddleWidth = collision.collider.bounds.size.x / 2f;
            float paddleAngle = hitPoint / paddleWidth;

            rb.linearVelocity = new Vector2(paddleAngle, 1f).normalized * speed;
        }

        if(collision.gameObject.CompareTag("Enemy"))
        {
            if(isLaunched)
            {
                Instantiate(hitParticles, transform.position, Quaternion.identity);
            }
        }

        Vector2 dir = rb.linearVelocity.normalized;

        if(Mathf.Abs(dir.y) < minYVelocity)
        {
            dir.y = Mathf.Sign(dir.y == 0 ? 1f : dir.y) * minYVelocity;
            dir.x = Mathf.Sign(dir.x) * Mathf.Sqrt(1f - dir.y * dir.y);

            rb.linearVelocity = dir * speed;
        }

        if (isLaunched && !collision.gameObject.CompareTag("Bottom") && !collision.gameObject.CompareTag("Enemy"))
        {
            hitSound.Play();
        }
    }
}
