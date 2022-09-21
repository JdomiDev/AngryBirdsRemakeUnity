using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip[] sound;

    [SerializeField]  private GameObject startMenu;
    [SerializeField]  private GameObject episodeMenu;
    [SerializeField]  private GameObject episode1;


    private void Start()
    {

        //startMenu = playButton.gameObject.transform.parent.gameObject;
        //episodeMenu = backButton.gameObject.transform.parent.gameObject;

    }

    private void PlayButton()
    {
        menuSound(1);
        closeWindow(startMenu);
        openWindow(episodeMenu);
    }
    private void EpisodeButton(GameObject episode)
    {
        menuSound(1);
        closeWindow(episodeMenu);
        openWindow(episode);
    }

    private void BackButton()
    {
        menuSound(0);

        if (episodeMenu.active)
        {
            closeWindow(episodeMenu);
            openWindow(startMenu);
        }
        else if(episode1.active)
        {
            closeWindow(episode1);
            openWindow(episodeMenu);
        }
    }
    private void levelButton(int Scene)
    {
        menuSound(1);
        SceneManager.LoadScene(Scene);
    }

    private void menuSound(int number)
    {
        audio.PlayOneShot(sound[number]);
    }
    private void quitGame()
    {
        Application.Quit();
    }
    private void closeWindow(GameObject Object)
    {
        Object.SetActive(false);
    }

    private void openWindow(GameObject Object)
    {
        Object.SetActive(true);
    }
}
