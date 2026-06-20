using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootData
{
    public int lootID;
    public string lootName;
    public int buyPrice;
    public int sellPrice;
    public string prefab;
}

public class LootDrop
{
    public int lootID;
    public float dropChance;
}

public class LootTable
{
    public int tableID;
    public List<LootDrop> entries = new();
}
