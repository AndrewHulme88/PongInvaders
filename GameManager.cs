using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Playing, Paused, GameOver }
    public bool isTransitioning { get; private set; }

    [SerializeField] private float endLevelDelay = 1f;

    public GameState currentState;
    public int playerLives = 3;
    public int score = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SetState(GameState.Playing);
        UIManager.Instance.UpdateLives(playerLives);
        UIManager.Instance.UpdateScore(score);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAllCoroutines();

        isTransitioning = false;
        Time.timeScale = 1f;
        SetState(GameState.Playing);

        if(UIManager.Instance != null)
        {
            UIManager.Instance.HidePauseScreen();
            UIManager.Instance.HideLevelEndScreen();
            UIManager.Instance.UpdateLives(playerLives);
            UIManager.Instance.UpdateScore(score);
        }
    }

    public void OnPause()
    {
        if (currentState == GameState.Playing)
        {
            SetState(GameState.Paused);
            UIManager.Instance.ShowPauseScreen();
        }
        else if (currentState == GameState.Paused)
        {
            SetState(GameState.Playing);
            UIManager.Instance.HidePauseScreen();
        }
    }

    public void LoseLife()
    {
        playerLives--;
        
        UIManager.Instance.UpdateLives(playerLives);

        if (playerLives <= 0)
        {
            SetState(GameState.GameOver);
        }
    }

    public void AddScore(int points)
    {
        score += points;
        UIManager.Instance.UpdateScore(score);
    }

    public void LevelEnd()
    {
        if (isTransitioning)
        {
            return;
        }

        StartCoroutine(EndLevelCoroutine());
    }

    private IEnumerator EndLevelCoroutine()
    {
        isTransitioning = true;
        FreezeGameplay();

        yield return new WaitForSeconds(endLevelDelay);

        UIManager.Instance.ShowLevelEndScreen(score, playerLives);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        isTransitioning = true;
        FreezeGameplay();
        yield return new WaitForSeconds(endLevelDelay);

        HighScoreManager.Instance.AddScore(score);
        UnfreezeGameplay();
        SceneManager.LoadScene("GameOver");
    }

    private void FreezeGameplay()
    {
        Ball ball = FindFirstObjectByType<Ball>();

        if (ball != null)
        {
            ball.FreezeBall();
        }
    }

    public void UnfreezeGameplay()
    {
        isTransitioning = false;
    }

    public void SetState(GameState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                break;

            case GameState.GameOver:
                GameOver();
                break;
        }
    }
}
