using UnityEngine;

public static class WeaponsList
{
    public static Weapon starter;

    public static void Init(GameObject starterBullet) 
    { 
        starter = new Weapon("Starter", 0, 10, 5f, starterBullet);
    }
}
