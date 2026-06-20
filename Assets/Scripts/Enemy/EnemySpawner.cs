using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private float spawnInterval = 2f;

    [SerializeField] private Transform player;
    [SerializeField] private float minSpawnDistance = 12f;
    [SerializeField] private float maxSpawnDistance = 18f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;

            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        int enemyID = GetRandomEnemyID();

        Vector3 spawnPoint = GetSpawnPosition();

        GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);

        EnemyBehaviour enemy = enemyObject.GetComponent<EnemyBehaviour>();

        enemy.enemyID = enemyID;
        enemy.InitializeEnemy();
    }


    private Vector3 GetSpawnPosition()
    {
        Vector2 dir = Random.insideUnitCircle.normalized;

        float dist = Random.Range(minSpawnDistance, maxSpawnDistance);

        return player.position + (Vector3)(dir * dist);
    }

    private int GetRandomEnemyID()
    {
        List<EnemyData> enemies = EnemyDatabase.Instance.GetAllEnemies();

        int totalWeight = 0;

        foreach (EnemyData enemy in enemies)
        {
            totalWeight += enemy.spawnWeight;
        }

        int roll = Random.Range(0, totalWeight);

        foreach (EnemyData enemy in enemies)
        {
            roll -= enemy.spawnWeight;

            if (roll < 0)
            {
                return enemy.enemyID;
            }
        }

        return enemies[0].enemyID;
    }
}