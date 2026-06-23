using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [SerializeField] private int fps;
    public bool hideCursor = false;

    private GameObject player;
    private PlayerHealth health;
    private Health playerHealth;

    private TextMeshProUGUI gameOverPrompt;

    //public PauseManager PM;

    public int Currency { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Application.targetFrameRate = fps;
        if (hideCursor)
        {
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        if (playerHealth.dead)
        {
            gameOverPrompt.text = "Game Over!\r\n\r\nPress 'R' to restart!";
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                ResetScene();
            }
        }
    }

    public void ResetScene()
    {
        Inventory.instance.ResetInventory();
        playerHealth.Initialize(health.maxHealth);
        Currency = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

        if (GameObject.Find("GameOverPrompt") != null)
        {
            gameOverPrompt = GameObject.Find("GameOverPrompt").GetComponent<TextMeshProUGUI>();
        }
        //PM.pausepanel = GameObject.Find("Pause Overlay").GetComponent<CanvasGroup>();
    }
}
