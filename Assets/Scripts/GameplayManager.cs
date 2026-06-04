using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pausepanel;

    private void Start()
    {
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Press P key
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;
        }

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        pausepanel.SetActive(true);
        Debug.Log("Game Paused");
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;
        pausepanel.SetActive(false);
        Debug.Log("Game Resumed");
    }

    public void PauseGame(bool isPause)
    {
        isPaused = isPause;
    }
}
