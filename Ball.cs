using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Transform startPoint;

    public float speed = 5f;
    
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ResetBall();
    }

    private void FixedUpdate()
    {
        if(rb.linearVelocity != Vector2.zero)
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

    void ResetBall()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = startPoint.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bottom"))
        {
            ResetBall();
        }
    }
}
