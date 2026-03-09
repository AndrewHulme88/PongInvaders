using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        ShowScore();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void ShowScore()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + GameManager.Instance.score;
        }
    }
}
