using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Playing, Paused, GameOver }

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
        Time.timeScale = 1f;
        SetState(GameState.Playing);
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
        SetState(GameState.Paused);
        UIManager.Instance.ShowLevelEndScreen(score, playerLives);
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
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
                Time.timeScale = 0f;
                GameOver();
                break;
        }
    }
}
