using System.Collections.Generic;
using UnityEngine;

public class EnemyDatabase : MonoBehaviour
{
    public static EnemyDatabase Instance;

    public Dictionary<int, EnemyData> enemyDatabase = new Dictionary<int, EnemyData>();

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

        LoadCSV();
    }

    void LoadCSV()
    {
        TextAsset enemyCSV = Resources.Load<TextAsset>("EnemyList");

        if (enemyCSV == null)
        {
            return;
        }

        string[] enemyRows = enemyCSV.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);

        for (int i = 1; i < enemyRows.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(enemyRows[i])) continue;

            string[] columns = enemyRows[i].Split(',');
            EnemyData enemy = new EnemyData {
                enemyID = int.Parse(columns[0]),
                enemyName = columns[1],
                baseHealth = int.Parse(columns[2]),
                baseSpeed = float.Parse(columns[3]),

                fireRate = float.Parse(columns[4]),
                turnSpeed = float.Parse(columns[5]),
                detectionRange = float.Parse(columns[6]),
                stoppingRange = float.Parse(columns[7]),

                contactDamage = int.Parse(columns[8]),
                knockbackForce = float.Parse(columns[9]),

                lootTableID = int.Parse(columns[10]),
                lootDropChance = float.Parse(columns[11]),

                multiplyMinCount = int.Parse(columns[12]),
                multiplyMaxCount = int.Parse(columns[13]),

                sprite = Resources.Load<Sprite>($"Sprites/Enemies/{columns[14]}")
            };

            enemyDatabase.Add(enemy.enemyID, enemy);
        }
    }

    public EnemyData GetEnemy(int lootID)
    {
        if (enemyDatabase.TryGetValue(lootID, out EnemyData enemy))
        {
            return enemy;
        }
        else
        {
            return null;
        }
    }

    public List<EnemyData> GetAllEnemies()
    {
        return new List<EnemyData>(enemyDatabase.Values);
    }
}
