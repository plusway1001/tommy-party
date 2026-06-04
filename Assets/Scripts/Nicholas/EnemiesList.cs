using UnityEngine;

public static class EnemiesList
{
    public static Enemy basic =
        new Enemy
        (   name: "Shooter",
            health: 50,
            speed: 2f,
            fireRate: 1f,
            turnSpeed: 7f,
            detectionRange: 10f,
            stoppingRange: 5f,
            isMelee: false,
            lootTable: new Loot[]
            {
                LootList.coin
            }
        );

    public static Enemy melee =
        new Enemy
        (name: "Shooter",
            health: 80,
            speed: 4f,
            fireRate: 0f,
            turnSpeed: 10f,
            detectionRange: 10f,
            stoppingRange: 0.5f,
            isMelee: true,
            lootTable: new Loot[]
            {
                LootList.coin
            }
        );
}
