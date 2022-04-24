using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public static int score;
    public static int highScore;
    public static GameObject scoreGo;
    float timeToIncreaseScore;

    private void Start()
    {
        scoreGo = GameObject.Find("Score");
        score = 0;
        if (PlayerPrefs.HasKey("highScore")) highScore = PlayerPrefs.GetInt("highScore");
        else highScore = 0;
        UpdateScore(0);
        timeToIncreaseScore = 60;
    }

    private void Update()
    {
        timeToIncreaseScore -= Time.deltaTime;
        if(timeToIncreaseScore <= 0)
        {
            UpdateScore(1);
            timeToIncreaseScore = 60;
        }
    }

    public static void UpdateScore(int amount)
    {
        score += amount;
        scoreGo.GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + score;
        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("highScore", highScore);
        }
    }

    public static void updateHighScoreDisplay(Scene current, Scene next)
    {
        if (next.buildIndex != 0) return;
        if(PlayerPrefs.HasKey("highScore")) highScore = PlayerPrefs.GetInt("highScore", highScore);
        else highScore = 0;
        GameObject.Find("HighScore").GetComponent<TMPro.TextMeshProUGUI>().text = "High Score  " + highScore;
    }
}
