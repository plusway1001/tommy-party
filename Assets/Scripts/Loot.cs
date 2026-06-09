using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootItem
{
    public string name;
    public int value;
    public float dropChance;
    public string prefabName;
}

public class LootTable
{
    public List<LootItem> items = new List<LootItem>();
}
