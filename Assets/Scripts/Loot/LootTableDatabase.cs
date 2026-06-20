using System.Collections.Generic;
using UnityEngine;

public class LootTableDatabase : MonoBehaviour
{
    public static LootTableDatabase Instance;

    public Dictionary<int, LootTable> lootTableDatabase = new();

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

    void LoadCSV()
    {
        TextAsset lootCSV = Resources.Load<TextAsset>("lootTableList");

        if (lootCSV == null)
        {
            return;
        }

        string[] lootRows = lootCSV.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);

        for (int i = 1; i < lootRows.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lootRows[i])) continue;

            string[] columns = lootRows[i].Split(',');

            int tableID = int.Parse(columns[0]);

            LootDrop item = new LootDrop
            {
                lootID = int.Parse(columns[1]),
                dropChance = float.Parse(columns[2])
            };

            if (!lootTableDatabase.ContainsKey(tableID))
            {
                lootTableDatabase[tableID] = new LootTable();
            }

            lootTableDatabase[tableID].entries.Add(item);
        }
    }

    public LootTable GetTable(int lootTableID)
    {
        if (lootTableDatabase.TryGetValue(lootTableID, out LootTable table))
        {
            return table;
        }
        else
        {
            return null;
        }
    }
}
