using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI scoreText;
    public int score = 0;
    public int maxScore = 0;

    public void scoreUpdate(int value)
    {
        score = score + value;
        scoreText.text = score.ToString();
    }


    public void maxScoreUpdate(int value)
    {
        maxScore = maxScore + value;
    }

}
