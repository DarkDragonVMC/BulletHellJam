using UnityEngine.UI;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static int score;
    float timeToIncreaseScore;

    private void Start()
    {
        score = 0;
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
        GameObject.Find("Score").GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + score;
    }
}
