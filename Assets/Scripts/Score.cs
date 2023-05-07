using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Text scoreTextPanel;
    [SerializeField] private Text scoreBest;

    private int score;
    private int bestScore;

    private void Start()
    {
        Load();
        ScoreTextUpdate();
    }

    private void ScoreSave()
    {
        PlayerPrefs.SetInt("score", score);

        if (score > PlayerPrefs.GetInt("bestScore"))
        {
            bestScore = score;
            PlayerPrefs.SetInt("bestScore", bestScore);
        }
    }

    private void Load()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            score = 0;
            PlayerPrefs.SetInt("score", score);
        }

        score = PlayerPrefs.GetInt("score");
        bestScore = PlayerPrefs.GetInt("bestScore");
    }

    private void ScoreTextUpdate()
    {
        scoreText.text = score.ToString();
        scoreTextPanel.text = ("SCORE: " + score);
        scoreBest.text = ("BEST SCORE: " + bestScore);
    }

    public void IncreasingScore(int setScore)
    {
        score += setScore;
        ScoreSave();
        ScoreTextUpdate();
    }

    public void ScoreUpdate()
    {
        ScoreSave();
        ScoreTextUpdate();
    }

    public void ResetScore()
    {
        ScoreSave();

        if (SceneManager.GetActiveScene().buildIndex == 0) score = 0;
        else score = PlayerPrefs.GetInt("beforescore");

        ScoreSave();
    }

    private void OnApplicationQuit()
    {
        ResetScore();
    }
}