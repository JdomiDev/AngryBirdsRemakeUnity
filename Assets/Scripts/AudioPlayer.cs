using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] public static AudioSource audio;

    private void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
    }
}
