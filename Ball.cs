using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Transform playerPaddle;
    [SerializeField] Vector3 ballOffset;

    public float speed = 5f;
    
    private Rigidbody2D rb;
    private bool isLaunched = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isLaunched = true;
                LaunchBall();
            }
        }
    }

    void LaunchBall()
    {
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
            GameManager.Instance.LoseLife();
            ResetBall();
        }

        if(collision.gameObject.CompareTag("Player"))
        {
            float hitPoint = transform.position.x - collision.transform.position.x;
            float paddleWidth = collision.collider.bounds.size.x / 2f;
            float paddleAngle = hitPoint / paddleWidth;

            rb.linearVelocity = new Vector2(paddleAngle, 1f).normalized * speed;
        }

        //if(collision.gameObject.CompareTag("Enemy"))
        //{
        //    Vector2 v = rb.linearVelocity;
        //    v.y = Mathf.Abs(v.y);
        //    rb.linearVelocity = v.normalized * speed;
        //}
    }
}
