using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    
    public static ItemDatabase Instance;

    private Dictionary<int, IDdata> items = new Dictionary<int, IDdata>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadCSV();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadCSV()
    {
        // Load ItemList.csv from Resources
        TextAsset itemCSV = Resources.Load<TextAsset>("ItemList");

        if (itemCSV == null)
        {
            Debug.LogError("ItemList.csv not found!");
            return;
        }

        // Split rows
        string[] rows = itemCSV.text.Split(
            new string[] { "\r\n", "\n" },
            System.StringSplitOptions.None
        );

        // Start at 1 to skip header
        for (int i = 1; i < rows.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(rows[i]))
                continue;

            // Split columns
            string[] columns = rows[i].Split(',');

            IDdata item = new IDdata();

            item.itemID = int.Parse(columns[0]);
            item.itemName = columns[1];

            // Load sprite from Resources/ItemImages
            item.itemImage = Resources.Load<Sprite>(
                "ItemImages/" + columns[2]
            );

            item.itemDescription = columns[3];

            // Add to dictionary using ID as key
            items.Add(item.itemID, item);
        }

        Debug.Log("Loaded " + items.Count + " items from CSV!");
    }

    // Get item data using ID
    public IDdata GetItem(int id)
    {
        if (items.ContainsKey(id))
        {
            return items[id];
        }

        Debug.LogWarning("Item ID " + id + " does not exist!");
        return null;
    }
}
