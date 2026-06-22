using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {  get; private set; }

    [SerializeField] private int fps;
    public static bool isPaused = false;

    public CanvasGroup pausepanel;

    private GameObject player;
    private PlayerHealth health;
    private Health playerHealth;

    private TextMeshProUGUI gameOverPrompt;

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
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        if (playerHealth.dead)
        {
            gameOverPrompt.text = "Game Over!\r\n\r\nPress 'R' to restart!";
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                Inventory.instance.ResetInventory();
                Scene currentScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(currentScene.name);
            }
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Inventory.instance.ResetInventory();
            playerHealth.Initialize(health.maxHealth);
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pausepanel.alpha = 1f;
        pausepanel.interactable = true;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausepanel.alpha = 0f;
        pausepanel.interactable = false;
        isPaused = false;
    }

    public void AddCurrency(int amount)
    {
        Currency += amount;
    }

    public void SetCurrency(int amount)
    {
        Currency = amount;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindWithTag("Player");
        health = FindFirstObjectByType<PlayerHealth>();
        playerHealth = FindFirstObjectByType<Health>();

        gameOverPrompt = GameObject.Find("GameOverPrompt").GetComponent<TextMeshProUGUI>();
        pausepanel = GameObject.Find("Pause Overlay").GetComponent<CanvasGroup>();
    }
}
