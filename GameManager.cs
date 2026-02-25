using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
        UIManager.Instance.UpdateLives(playerLives);
        UIManager.Instance.UpdateScore(score);
    }

    public void LoseLife()
    {
        playerLives--;
        
        UIManager.Instance.UpdateLives(playerLives);

        if (playerLives <= 0)
        {
            GameOver();
        }
    }

    public void AddScore(int points)
    {
        score += points;
        UIManager.Instance.UpdateScore(score);
    }

    private void GameOver()
    {
        Debug.Log("Game Over! Final Score: " + score);
    }
}
