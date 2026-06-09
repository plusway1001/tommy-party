using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {  get; private set; }

    [SerializeField] private int fps;
    public static bool isPaused = false;

    public GameObject pausepanel;

    public int Currency { get; private set; }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Application.targetFrameRate = fps;
        //Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        pausepanel.SetActive(true);
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;
        pausepanel.SetActive(false);
    }

    public void PauseGame(bool isPause)
    {
        isPaused = isPause;
    }

    public void AddCurrency(int amount)
    {
        Currency += amount;
    }
}
