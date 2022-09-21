using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using TMPro;
using System.Collections;

public class InGameMenu : MonoBehaviour
{

    [SerializeField] private AudioSource audio;

    [SerializeField] private AudioClip[] sound;

    [SerializeField] private GameObject defaultState;
    [SerializeField] private GameObject openState;
    [SerializeField] private GameObject wonState;
    [SerializeField] private GameObject lostState;
    [SerializeField] private GameObject scoreState;
    [SerializeField] private GameObject highScore;
    [SerializeField] private GameObject[] stars;
    [SerializeField] private GameObject[] emptyStars;

    [SerializeField] private AudioClip[] lostSounds;
    [SerializeField] private AudioClip[] winSounds;
    [SerializeField] private AudioClip scoreCountLoop;


    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highscoreText;


    private List<GameObject> Birds;


    Score score;


    private bool onWinScreen = false;


    private void Start()
    {
        if(PlayerPrefs.HasKey(SceneManager.GetActiveScene().buildIndex + "h"))
        {
            highscoreText.text = PlayerPrefs.GetInt(SceneManager.GetActiveScene().buildIndex + "h").ToString();
        }
        score = GameObject.Find("number").GetComponent<Score>();
    }

    private void Awake()
    {
        Birds = new List<GameObject>(GameObject.FindGameObjectsWithTag("Bird"));
    }

    private void FixedUpdate()
    {
        if (GameManager.CurrentGameState == GameState.Won && onWinScreen == false && GameManager.BricksBirdsPigsStoppedMoving() && new List<GameObject>(GameObject.FindGameObjectsWithTag("Bird")).Count == GameManager.birdsNumber)
        {
            int rand = Random.Range(0, 2);
            if (!audio.isPlaying)
            {
                AudioPlayer.audio.PlayOneShot(winSounds[rand]);
            }
            GameObject.Find("ambient").GetComponent<AudioSource>().Stop();
            StartCoroutine(winDelay(rand));
        }
        else if(GameManager.CurrentGameState == GameState.Lost && onWinScreen == false && GameManager.BricksBirdsPigsStoppedMoving() && new List<GameObject>(GameObject.FindGameObjectsWithTag("Bird")).Count == GameManager.birdsNumber)
        {
            GameObject.Find("ambient").GetComponent<AudioSource>().Stop();
            StartCoroutine(lostDelay());
        }
    }

    private void calculateStars()
    {
        score.maxScoreUpdate((Mathf.RoundToInt(Birds.Count * 0.75f)) * 10000);
        score.scoreUpdate((new List<GameObject>(GameObject.FindGameObjectsWithTag("Bird")).Count) * 10000);

        if (((float)score.score / (float)score.maxScore) * 100f >= 80f) // 3 starts
        {

            StartCoroutine(starDelay(3));

            if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().buildIndex.ToString()) < 3)
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().buildIndex.ToString(), 3);
            }

        }
        else if(((float)score.score / (float)score.maxScore) * 100f >= 50f) // 2 stars
        {
            StartCoroutine(starDelay(2));

            if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().buildIndex.ToString()) < 2)
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().buildIndex.ToString(), 2);
            }
        }
        else // 1 star
        {
            StartCoroutine(starDelay(1));

            if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().buildIndex.ToString()) < 1)
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().buildIndex.ToString(), 1);
            }
        }
        PlayerPrefs.Save();
        StartCoroutine(CountUpToTarget(scoreText, score.score, PlayerPrefs.GetInt(SceneManager.GetActiveScene().buildIndex.ToString())));

    }

    private void highScoreShow()
    {
        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().buildIndex.ToString() + "h"))
        {
            if (PlayerPrefs.GetInt("1h") < score.score)
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().buildIndex.ToString() + "h", score.score);
                highScore.SetActive(true);
                menuSound(3);
            }
        }
        else
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().buildIndex.ToString() + "h", score.score);
            highScore.SetActive(true);
            menuSound(3);
        }
        PlayerPrefs.Save();
    }
    private void changeState()
    {
        menuSound(1);
        if (defaultState.active == true)
        {
            defaultState.SetActive(false);
            scoreState.SetActive(false);
            openState.SetActive(true);
        }
        else if(openState.active == true)
        {
            openState.SetActive(false);
            scoreState.SetActive(true);
            defaultState.SetActive(true);
        }
    }
    private void menuSound(int number)
    {
        audio.PlayOneShot(sound[number]);
    }

    private void reloadLevel()
    {
        menuSound(1);
        Application.LoadLevel(Application.loadedLevel);
    }
    private void loadMenu()
    {
        menuSound(1);
        SceneManager.LoadScene(0);
    }

    private void nextLevel()
    {
        menuSound(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }





    // enums 


    private IEnumerator lostDelay()
    {
        yield return new WaitForSeconds(2f);

        if (GameManager.CurrentGameState == GameState.Lost && onWinScreen == false && GameManager.BricksBirdsPigsStoppedMoving() && new List<GameObject>(GameObject.FindGameObjectsWithTag("Bird")).Count == GameManager.birdsNumber)
        {
            AudioPlayer.audio.PlayOneShot(lostSounds[Random.Range(0, 2)]);


            onWinScreen = true;

            defaultState.SetActive(false);
            scoreState.SetActive(false);
            openState.SetActive(false);
            lostState.SetActive(true);
        }
    }
    private IEnumerator winDelay(int clipIndex)
    {
        yield return new WaitForSeconds(winSounds[clipIndex].length);
        if (GameManager.CurrentGameState == GameState.Won && onWinScreen == false && GameManager.BricksBirdsPigsStoppedMoving() && new List<GameObject>(GameObject.FindGameObjectsWithTag("Bird")).Count == GameManager.birdsNumber)
        {

            onWinScreen = true;

            defaultState.SetActive(false);
            openState.SetActive(false);
            scoreState.SetActive(false);
            wonState.SetActive(true);

            calculateStars();
        }
        else
        {
            StartCoroutine(lostDelay());
        }
    }


    private IEnumerator starDelay(int num)
    {
        int loopCount = 0;

        int starIndex = 0;

        yield return new WaitForSeconds(1f);
        while (num > loopCount)
        {
            stars[starIndex].SetActive(true);
            emptyStars[starIndex].SetActive(false);

            loopCount++;
            starIndex++;

            yield return new WaitForSeconds(1f);
        }
        highScoreShow();
    }
    IEnumerator CountUpToTarget(TextMeshProUGUI label, int targetVal, float duration, float delay = 0f, string prefix = "")
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }

        int current = 0;
        while (current < targetVal)
        {
            if (AudioPlayer.audio.clip != scoreCountLoop && !AudioPlayer.audio.isPlaying)
            {
                AudioPlayer.audio.PlayOneShot(scoreCountLoop);
            }

            current += (int)(targetVal / (duration / Time.deltaTime));
            current = Mathf.Clamp(current, 0, targetVal);
            label.text = prefix + current;
            yield return null;
        }
    }

}
