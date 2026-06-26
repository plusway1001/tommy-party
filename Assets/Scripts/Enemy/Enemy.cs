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

    public int contactDamage;
    public float knockbackForce;

    public int lootTableID;
    public float lootDropChance;

    public int multiplyMinCount;
    public int multiplyMaxCount;

    //public Sprite sprite;
    public Sprite[] sprites;
    public float animationSpeed = 0.1f; // seconds per frame
}
