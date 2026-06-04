using UnityEngine;

public static class LootList
{
    public static Loot coin;

    public static void Init(Sprite coinSprite)
    {
        coin = new Loot(
            name: "Coin",
            value: 1,
            dropChance: 0.8f,
            icon: coinSprite
        );
    }
}
