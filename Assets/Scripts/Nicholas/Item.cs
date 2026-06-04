using UnityEngine;

public enum ItemType
{
    Weapon,
    Loot,
    Consumable
}

public class Item
{
    public string name;
    public ItemType type;
    public int value;
}
