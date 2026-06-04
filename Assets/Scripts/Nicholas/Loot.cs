using UnityEngine;

public class Loot : Item
{
    public float dropChance;
    public Sprite icon;

    public Loot(string name, int value, float dropChance, Sprite icon)
    {
        this.name = name;
        this.value = value;
        this.dropChance = dropChance;
        this.icon = icon;
    }
}
