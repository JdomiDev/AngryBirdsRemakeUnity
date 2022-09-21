using UnityEngine;
using TMPro;

public class EpisodeScores : MonoBehaviour
{
    [SerializeField] GameObject episodeLevels;

    [SerializeField] TextMeshProUGUI maxStarText;

    [SerializeField] TextMeshProUGUI totalStarText;

    [SerializeField] TextMeshProUGUI totalScoreText;

    [HideInInspector]
    public int levelsCount;
    [HideInInspector]
    public int totalStarCount;
    [HideInInspector]
    public int totalMaxStarCount;
    [HideInInspector]
    public int totalScoreCount;



    private void Start()
    {
        levelsCount = episodeLevels.transform.childCount - 1;
        totalMaxStarCount = levelsCount *3;

        int i = 0;

        while(levelsCount != i)
        {
            i++;
            if(PlayerPrefs.HasKey(i.ToString()))
            {
                totalStarCount = totalStarCount + PlayerPrefs.GetInt(i.ToString());
                totalScoreCount = totalScoreCount + PlayerPrefs.GetInt(i.ToString() + "h");
            }

        }
        maxStarText.text = totalMaxStarCount.ToString();
        totalStarText.text = totalStarCount.ToString();
        totalScoreText.text = totalScoreCount.ToString();
    }
}
