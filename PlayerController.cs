using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float invulnerabilityDuration = 0.75f;
    [SerializeField] private Ball ball;
    [SerializeField] private GameObject hitParticles;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isInvulnerable = false;
    private AudioSource hitSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        hitSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(GameManager.Instance.currentState != GameManager.GameState.Playing)
        {
            return;
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, 0f);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        moveInput = input.x;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(!context.performed)
        {
            return;
        }

        if(UIManager.Instance.levelEndPanel.activeInHierarchy)
        {
            UIManager.Instance.LoadNextLevel();
            return;
        }

        if (ball != null)
        {
            ball.LaunchBall();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet") && !isInvulnerable)
        {
            hitSound.Play();
            FindFirstObjectByType<CameraController>().ShakeCamera();
            StartCoroutine(HitRoutine());

            Instantiate(hitParticles, transform.position, Quaternion.identity);

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
