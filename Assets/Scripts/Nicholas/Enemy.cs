using UnityEngine;

public class Enemy
{
    public string name;
    public int health;
    public float speed;
    public float fireRate;
    public float turnSpeed;
    public float detectionRange;
    public float stoppingRange;
    public bool isMelee;

    public Loot[] lootTable;

    public Enemy(string name, int health, float speed, float fireRate, float turnSpeed, float detectionRange, float stoppingRange, bool isMelee, Loot[] lootTable)
    {
        this.name = name;
        this.health = health;
        this.speed = speed;
        this.fireRate = fireRate;
        this.turnSpeed = turnSpeed;
        this.detectionRange = detectionRange;
        this.stoppingRange = stoppingRange;
        this.isMelee = isMelee;
        this.lootTable = lootTable;
    }
}
