using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject levelEndPanel;

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelEndScoreText;
    public TextMeshProUGUI levelEndLivesText;

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

    public void ShowLevelEndScreen(int score, int lives)
    {
        levelEndPanel.SetActive(true);
        levelEndScoreText.text = "Score: " + score;
        levelEndLivesText.text = "Lives: " + lives;
    }

    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(nextSceneIndex);
            levelEndPanel.SetActive(false);
        }
        else
        {
            Debug.Log("No more levels! Returning to main menu.");
            LoadMainMenu();
        }
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
