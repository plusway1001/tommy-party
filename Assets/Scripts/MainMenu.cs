using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Load selected scene
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Quit game
    public void QuitGame()
    {
        Debug.Log("Game Quit");

        Application.Quit();
    }
}
