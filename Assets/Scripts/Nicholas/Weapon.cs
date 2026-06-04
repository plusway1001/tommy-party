using UnityEngine;

public class Weapon : Item
{
    public int damage;
    public GameObject bulletPrefab;
    public float fireRate;

    public Weapon(string name, int value, int damage, float fireRate, GameObject bulletPrefab)
    {
        this.name = name;
        this.value = value;
        this.damage = damage;
        this.fireRate = fireRate;
        this.bulletPrefab = bulletPrefab;
    }
}
