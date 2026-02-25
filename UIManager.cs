using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject pausePanel;

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        pausePanel.SetActive(false);
    }

    public void UpdateLives(int lives)
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + lives;
        }
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    public void ShowPauseScreen()
    {
        if (pausePanel == null)
        {
            return;
        }

        pausePanel.SetActive(true);
    }

    public void HidePauseScreen()
    {
        if (pausePanel == null)
        {
            return;
        }

        GameManager.Instance.SetState(GameManager.GameState.Playing);
        pausePanel.SetActive(false);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowGameOver()
    {
        Debug.Log("Game Over! Final Score: " + scoreText.text);
    }
}
