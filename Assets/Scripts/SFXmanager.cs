using UnityEngine;
using UnityEngine.SceneManagement;

public class SFXmanager : MonoBehaviour
{
    public static SFXmanager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // SFX
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
