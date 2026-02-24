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

    public void LoseLife()
    {
        playerLives--;
        Debug.Log("Player Lives: " + playerLives);
        if (playerLives <= 0)
        {
            GameOver();
        }
    }

    public void AddScore(int points)
    {
        score += points;
    }

    private void GameOver()
    {
        Debug.Log("Game Over! Final Score: " + score);
    }
}
