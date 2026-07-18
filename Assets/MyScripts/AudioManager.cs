using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    //public AudioSource ambientSource;
    public AudioSource effectsSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayEffect(AudioClip clip)
    {
        effectsSource.PlayOneShot(clip);
    }
}
