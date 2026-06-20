using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<int, int> items = new();

    public static Inventory instance { get; set; }

    public event Action<int, int> OnItemChanged;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddItem(int lootID, int amount)
    {
        LootData loot = LootDatabase.Instance.GetLoot(lootID);

        if (loot == null)
        {
            return;
        }

        if (loot.lootName == "Coin")
        {
            GameManager.instance.AddCurrency(amount);
            return;
        }

        if (!items.ContainsKey(lootID))
        {
            items[lootID] = 0;
        }

        items[lootID] += amount;
        OnItemChanged?.Invoke(lootID, items[lootID]);
    }

    public bool RemoveItem(int lootID, int amount)
    {
        if (!items.ContainsKey(lootID))
        {
            return false;
        }

        if (items[lootID] < amount)
        {
            return false;
        }

        items[lootID] -= amount;

        if (items[lootID] <= 0)
        {
            items.Remove(lootID);
        }

        OnItemChanged?.Invoke(lootID, GetItemCount(lootID));

        return true;
    }

    public int GetItemCount(int lootID)
    {
        if (items.TryGetValue(lootID, out int count))
        {
            return count;
        }
        else
        {
            return 0;
        }
    }

    public bool HasItem(int lootID)
    {
        return GetItemCount(lootID) > 0;
    }

    public Dictionary<int, int> GetAllItems()
    {
        return items;
    }
}
