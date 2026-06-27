using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private Transform player;
    [SerializeField] private float minSpawnDistance = 12f;
    [SerializeField] private float maxSpawnDistance = 18f;

    private Dictionary<int, int> aliveByType = new();

    [SerializeField] private GameObject nextWavePrompt;
    [SerializeField] private TextMeshProUGUI nextWavePromptText;
    private bool waitingForNextWave;

    [SerializeField] private int currentWave = 1;
    public int aliveEnemies;
    private int finalWave;

    [SerializeField] private GameObject winPrompt;
    [SerializeField] private TextMeshProUGUI winPromptText;
    private bool gameWon;

    private List<ActiveSpawnGroup> activeGroups = new();
    private bool waveStarted = false;

    [SerializeField] private AudioClip[] EnemySpawnSound;

    private class ActiveSpawnGroup
    {
        public WaveSpawnData data;
        public int remaining;
        public float timer;
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        finalWave = WaveDatabase.Instance.GetFinalWave();
        LoadWave(currentWave);
    }

    private void Update()
    {
        if (waitingForNextWave)
        {
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                waitingForNextWave = false;
                if (nextWavePrompt != null) nextWavePrompt.SetActive(false);
                currentWave++;
                LoadWave(currentWave);
            }
            return;
        }

        if (gameWon)
        {
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Time.timeScale = 1f;
                GameManager.instance.ResetScene();
            }
            return;
        }

        for (int i = activeGroups.Count - 1; i >= 0; i--)
        {
            ActiveSpawnGroup group = activeGroups[i];
            group.timer += Time.deltaTime;

            if (group.timer >= group.data.spawnInterval)
            {
                int aliveOfType = aliveByType.GetValueOrDefault(group.data.enemyID);

                if (aliveOfType < group.data.maxAlive)
                {
                    group.timer = 0f;
                    SpawnEnemy(group.data.enemyID);
                    group.remaining--;

                    if (group.remaining <= 0)
                        activeGroups.RemoveAt(i);
                }
            }
        }

        if (WaveFinished())
        {
            StartNextWave();
        }
    }

    private bool WaveFinished()
    {
        return waveStarted && aliveEnemies <= 0 && activeGroups.Count == 0;
    }

    public void OnEnemyKilled(int enemyID)
    {
        aliveEnemies--;

        if (aliveByType.ContainsKey(enemyID))
        {
            aliveByType[enemyID]--;
        }

        if (WaveFinished())
        {
            StartNextWave();
        }
    }

    private void SpawnEnemy(int enemyID)
    {
        Vector3 spawnPoint = GetSpawnPosition();
        GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        EnemyBehaviour enemy = enemyObject.GetComponent<EnemyBehaviour>();
        enemy.enemyID = enemyID;
        enemy.InitializeEnemy();

        AudioManager.Instance.PlaySFX(EnemySpawnSound[Random.Range(0, EnemySpawnSound.Length)]);

        aliveEnemies++;
        aliveByType.TryAdd(enemyID, 0);
        aliveByType[enemyID]++;
    }

    private Vector3 GetSpawnPosition()
    {
        Vector2 dir = Random.insideUnitCircle.normalized;
        float dist = Random.Range(minSpawnDistance, maxSpawnDistance);
        if (player != null) return player.position + (Vector3)(dir * dist);
        return Vector3.zero;
    }

    private void LoadWave(int waveID)
    {
        activeGroups.Clear();
        waveStarted = false;
        aliveEnemies = 0;
        aliveByType.Clear();

        WaveData wave = WaveDatabase.Instance.GetWave(waveID);
        if (wave == null)
        {
            return;
        }

        foreach (var spawn in wave.spawns)
        {
            activeGroups.Add(
                new ActiveSpawnGroup
            {
                data = spawn,
                remaining = spawn.spawnCount,
                timer = spawn.spawnInterval
            });
        }

        waveStarted = true;
    }

    private void StartNextWave()
    {
        if (currentWave >= finalWave)
        {
            WinGame();
            return;
        }

        waitingForNextWave = true;
        if (nextWavePrompt != null) nextWavePrompt.SetActive(true);
        if (nextWavePromptText != null)
        {
            nextWavePromptText.text = $"Wave {currentWave} Complete\n\nPress SPACE to start Wave {currentWave + 1}";
        }
    }

    private void WinGame()
    {
        gameWon = true;
        if (winPrompt != null) winPrompt.SetActive(true);
        if (winPromptText != null)
        {
            winPromptText.text = $"You survived all {finalWave} waves!\n\nPress SPACE to Restart";
        }
        Time.timeScale = 0f;
    }
}