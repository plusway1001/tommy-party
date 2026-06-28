using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Load selected scene
    public void LoadScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    // Quit game
    public void QuitGame()
    {

        Application.Quit();
    }
}
