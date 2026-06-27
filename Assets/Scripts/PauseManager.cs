using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;

    public CanvasGroup pausepanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResumeGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            isPaused = !isPaused;
        }

        if (TutorialManager.hasShownTutorial)
        {
            if (!isPaused)
            {
                Time.timeScale = 1f;
                pausepanel.alpha = 0f;
                pausepanel.interactable = false;
            }
            else
            {
                Time.timeScale = 0f;
                pausepanel.alpha = 1f;
                pausepanel.interactable = true;
            }
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
