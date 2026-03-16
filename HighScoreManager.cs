using UnityEngine;
using System.Collections.Generic;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance { get; private set; }

    private const int MaxScores = 5;
    private const string HighScoreKey = "HighScores";

    private List<int> highScores = new List<int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadHighScores();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int newScore)
    {
        highScores.Add(newScore);
        highScores.Sort((a, b) => b.CompareTo(a)); // Sort in descending order

        if (highScores.Count > MaxScores)
        {
            highScores.RemoveRange(MaxScores, highScores.Count - MaxScores); // Keep only top scores
        }

        SaveHighScores();
    }

    public List<int> GetHighScores()
    {
        return highScores;
    }

    private void SaveHighScores()
    {
        string data = string.Join(",", highScores);
        PlayerPrefs.SetString(HighScoreKey, data);
        PlayerPrefs.Save();
    }

    private void LoadHighScores()
    {
        highScores.Clear();

        if(!PlayerPrefs.HasKey(HighScoreKey))
        {
            return;
        }

        string data = PlayerPrefs.GetString(HighScoreKey);

        if(string.IsNullOrEmpty(data))
        {
            return;
        }

        string[] split = data.Split(',');

        foreach (string score in split)
        {
            if (int.TryParse(score, out int parsedScore))
            {
                highScores.Add(parsedScore);
            }
        }

        highScores.Sort((a, b) => b.CompareTo(a)); // Ensure scores are sorted
    }

    public void ClearHighScores()
    {
        highScores.Clear();
        PlayerPrefs.DeleteKey(HighScoreKey);
    }
}
