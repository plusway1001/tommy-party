using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public CanvasGroup Tutorialpanel;
    public static bool hasShownTutorial = false;

    void Start()
    {
        // This runs EXACTLY ONCE right when Level 1 starts up
        ShowTutorial();
    }

    void ShowTutorial()
    {
        if (!hasShownTutorial)
        {
            UnityEngine.SceneManagement.Scene targetScene = SceneManager.GetSceneByName("Level 1");
            if (targetScene.isLoaded)
            {
                Time.timeScale = 0f;
                Tutorialpanel.alpha = 1f;
                Tutorialpanel.interactable = true;
                Tutorialpanel.blocksRaycasts = true; // Ensures the panel blocks clicks to the game behind it

            }
        }
        else
        {
            Tutorialpanel.blocksRaycasts = false; // When game replays, disable raycast block
        }
    }
    public void closetutorial()
    {
        Debug.Log("play is clicked");
        Time.timeScale = 1f;
        Tutorialpanel.alpha = 0f;
        Tutorialpanel.interactable = false;
        Tutorialpanel.blocksRaycasts = false; // Allows player to click the game again
        Tutorialpanel.gameObject.SetActive(false);
        hasShownTutorial = true;
    }
}
