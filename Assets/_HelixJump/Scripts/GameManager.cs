using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    public int best;
    public int score;
    public int currentLevel = 0;

    [Header("UI")]
    public GameObject gameOverPanel;
    public GameObject gameUI;
    public GameObject Helix;
    public GameObject ball;
    public TMP_Text finalScoreText;
    public TMP_Text bestScoreText;

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else if (singleton != this)
        {
            Destroy(gameObject);
        }

        best = PlayerPrefs.GetInt("BestScore");
    }

    public void NextLevel()
    {
        Debug.Log("NextLevel Called");
    }

    public void RestartLevel()
    {
        Debug.Log("Restart Level");

        score = 0;
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;

        if (score > best)
        {
            best = score;
            PlayerPrefs.SetInt("BestScore", best);
        }
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0f;

        finalScoreText.text = "Score: " + score;
        bestScoreText.text = "Best: " + best;
        ball.SetActive(false);
        Helix.SetActive(false);
        gameUI.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    
}
