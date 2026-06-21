using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {  get; private set; }

    [SerializeField] private int fps;
    public static bool isPaused = false;

    public GameObject pausepanel;

    [SerializeField] private GameObject player;
    [SerializeField] private PlayerHealth health;
    [SerializeField] private Health playerHealth;

    [SerializeField] private GameObject gameOverPrompt;

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
        Cursor.visible = false;
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
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                player.transform.position = player.GetComponent<PlayerMovement>().initialPos;
                playerHealth.dead = false;
                playerHealth.Initialize(health.maxHealth);
                player.SetActive(true);

                gameOverPrompt.SetActive(false);

                EnemySpawner.Instance.ResetWaves();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pausepanel.SetActive(true);
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausepanel.SetActive(false);
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
}
