using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int best;
    public int score;

    public int currentLevel = 0;


    public static GameManager singleton;

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }else if (singleton != this)
        {
            Destroy(gameObject);
        }

        best = PlayerPrefs.GetInt("HighScore");
    }

    public void NextLevel()
    {
        Debug.Log("NextLlevel Called");
    }

    public void RestartLevel()
    {
        Debug.Log("GameOver");
        singleton.score = 0;
        FindAnyObjectByType<BallController>().ResetBall();
        //reload
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;

        if (score > best)
        {
            best = score;

            PlayerPrefs.SetInt("HighScore" , score);
        }
    }

}
