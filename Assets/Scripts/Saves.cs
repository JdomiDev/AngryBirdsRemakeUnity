using UnityEngine;

public class Saves : MonoBehaviour
{
    [SerializeField] private int levelIndex;
    [SerializeField] private GameObject zeroStar;
    [SerializeField] private GameObject oneStar;
    [SerializeField] private GameObject twoStars;
    [SerializeField] private GameObject threeStars;

    private void Awake()
    {
        if(PlayerPrefs.HasKey(levelIndex.ToString()))
        {
            if(PlayerPrefs.GetInt(levelIndex.ToString()) == 1)
            {
                oneStar.SetActive(true);
            }
            else if(PlayerPrefs.GetInt(levelIndex.ToString()) == 2)
            {
                twoStars.SetActive(true);
            }
            else if (PlayerPrefs.GetInt(levelIndex.ToString()) == 3)
            {
                threeStars.SetActive(true);
            }
            else
            {
                zeroStar.SetActive(true);
            }
        }
        else
        {
            zeroStar.SetActive(true);
        }
    }
}
