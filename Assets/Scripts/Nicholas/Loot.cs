using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootItem
{
    public string name;
    public int value;
    public float dropChance;
    public string icon;
}

public class LootTable
{
    public List<LootItem> items = new List<LootItem>();
}
