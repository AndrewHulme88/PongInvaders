using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Unity.Jobs;

public class HighScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void OnEnable()
    {
        RefreshScores();
    }

    private void RefreshScores()
    {
        if(HighScoreManager.Instance == null || highScoreText == null)
        {
            return;
        }

        List<int> scores = HighScoreManager.Instance.GetHighScores();

        if (scores.Count == 0)
        {
            highScoreText.text = "No high scores yet!";
            return;
        }

        highScoreText.text = "";

        for(int i = 0; i < scores.Count; i++)
        {
            highScoreText.text += $"{i + 1}. {scores[i]}\n";
        }
    }

    public void ClearHighScores()
    {
        if (HighScoreManager.Instance != null)
        {
            HighScoreManager.Instance.ClearHighScores();
            RefreshScores();
        }
    }
}
