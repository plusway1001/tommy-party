using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pausepanel;

    private void Start()
    {
        isPaused = false;
    }

    private void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            isPaused = !isPaused;
        }

        CheckforGamePause();
    }

    public void CheckforGamePause()
    {
        if (!isPaused)
        {
            Time.timeScale = 1f;
            pausepanel.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            pausepanel.SetActive(true);
        }
    }

    public void PauseGame()
    {
        isPaused = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
    }
}
