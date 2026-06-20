using System.Collections.Generic;
using UnityEngine;

public class LootDatabase : MonoBehaviour
{
    public static LootDatabase Instance;

    public Dictionary<int, LootData> lootDatabase = new Dictionary<int, LootData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        LoadCSV();
    }

    private void LoadCSV()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("lootList");

        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            string[] values = lines[i].Split(',');

            LootData loot = new LootData
            {
                lootID = int.Parse(values[0]),
                lootName = values[1],
                buyPrice = int.Parse(values[2]),
                sellPrice = int.Parse(values[3]),
                prefab = values[4].Trim()
            };

            lootDatabase.Add(loot.lootID, loot);
        }
    }

    public LootData GetLoot(int lootID)
    {
        if (lootDatabase.TryGetValue(lootID, out LootData loot)) 
        {
            return loot;
        }
        else
        {
            return null;
        }
    }

    public IEnumerable<LootData> GetAllLoot()
    {
        return lootDatabase.Values;
    }
}