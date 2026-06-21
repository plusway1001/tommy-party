using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private Transform player;
    [SerializeField] private float minSpawnDistance = 12f;
    [SerializeField] private float maxSpawnDistance = 18f;

    private Queue<WaveSpawnData> spawnQueue = new();

    private WaveSpawnData currentSpawn;
    private int remainingCount;
    private float timer;

    [SerializeField] private GameObject nextWavePrompt;
    [SerializeField] private TextMeshProUGUI promptText;

    private bool waitingForNextWave;

    [SerializeField] private int currentWave = 1;

    private int aliveEnemies;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadWave(currentWave);
    }

    private void Update()
    {
        if (waitingForNextWave)
        {
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                waitingForNextWave = false;

                if (nextWavePrompt != null)
                {
                    nextWavePrompt.SetActive(false);
                }

                currentWave++;
                LoadWave(currentWave);
            }

            return;
        }

        if (currentSpawn == null)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= currentSpawn.spawnInterval)
        {
            timer = 0f;

            SpawnEnemy(currentSpawn.enemyID);

            remainingCount--;

            if (remainingCount <= 0)
            {
                StartNextSpawnGroup();
            }
        }
    }

    private bool WaveFinished()
    {
        return aliveEnemies <= 0 && currentSpawn == null && spawnQueue.Count == 0;
    }

    public void OnEnemyKilled()
    {
        aliveEnemies--;

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

        aliveEnemies++;
    }


    private Vector3 GetSpawnPosition()
    {
        Vector2 dir = Random.insideUnitCircle.normalized;

        float dist = Random.Range(minSpawnDistance, maxSpawnDistance);

        return player.position + (Vector3)(dir * dist);
    }

    private void LoadWave(int waveID)
    {
        spawnQueue.Clear();

        WaveData wave = WaveDatabase.Instance.GetWave(waveID);

        if (wave == null)
        {
            return;
        }

        foreach (var spawn in wave.spawns)
        {
            spawnQueue.Enqueue(spawn);
        }

        StartNextSpawnGroup();
    }

    private void StartNextSpawnGroup()
    {
        if (spawnQueue.Count == 0)
        {
            currentSpawn = null;

            if (WaveFinished())
            {
                StartNextWave();
            }

            return;
        }

        currentSpawn = spawnQueue.Dequeue();
        remainingCount = currentSpawn.spawnCount;
        timer = 0f;
    }

    private void StartNextWave()
    {
        waitingForNextWave = true;

        if (nextWavePrompt != null)
        {
            nextWavePrompt.SetActive(true);
        }

        if (promptText != null)
        {
            promptText.text =
                $"Wave {currentWave} Complete\n\nPress SPACE to start Wave {currentWave + 1}";
        }
    }
}