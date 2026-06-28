using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public CanvasGroup Tutorialpanel;
    public static bool hasShownTutorial = false;

    void Start()
    {
        ShowTutorial();
    }

    void ShowTutorial()
    {
        if (!hasShownTutorial)
        {
            UnityEngine.SceneManagement.Scene targetScene = SceneManager.GetSceneByBuildIndex(1);
            if (targetScene.isLoaded)
            {
                Time.timeScale = 0f;
                Tutorialpanel.alpha = 1f;
                Tutorialpanel.interactable = true;
                Tutorialpanel.blocksRaycasts = true;

            }
        }
        else
        {
            Tutorialpanel.blocksRaycasts = false;
        }
    }
    public void closetutorial()
    {
        Time.timeScale = 1f;
        Tutorialpanel.alpha = 0f;
        Tutorialpanel.interactable = false;
        Tutorialpanel.blocksRaycasts = false;
        Tutorialpanel.gameObject.SetActive(false);
        hasShownTutorial = true;
    }
}
