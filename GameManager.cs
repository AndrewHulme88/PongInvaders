using UnityEngine;
using System.Collections;

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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
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
                UIManager.Instance.ShowGameOver();
                break;
        }
    }
}
