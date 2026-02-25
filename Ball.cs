using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Transform ballStartPoint;

    public float speed = 5f;
    
    private Rigidbody2D rb;

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
        if(rb.linearVelocity == Vector2.zero && Input.GetKeyDown(KeyCode.Space))
        {
            LaunchBall();
        }
    }

    void LaunchBall()
    {
        Vector2 direction = new Vector2(Random.Range(-0.5f, 0.5f), -1).normalized;
        rb.linearVelocity = direction * speed;
    }

    public void ResetBall()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = ballStartPoint.position;
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
