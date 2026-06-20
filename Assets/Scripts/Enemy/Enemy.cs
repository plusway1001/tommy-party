using System.Collections.Generic;
using UnityEngine;
    
[System.Serializable]
public class EnemyData
{
    public int enemyID;
    public string enemyName;
    public int baseHealth;
    public float baseSpeed;
    public float fireRate;
    public float turnSpeed;
    public float detectionRange;
    public float stoppingRange;

    public int lootTableID;
    public int spawnWeight;

    public Sprite sprite;
}
