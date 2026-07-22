using UnityEngine;

public class TriggerSFX : MonoBehaviour
{
    // Update is called once per frame
    public static void PlayAudioSFX(AudioClip clip)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(clip);
        }
        else
        {
            Debug.LogWarning("AudioManager instance is missing from the scene!");
        }
    }
}
