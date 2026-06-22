using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private InventorySlotUI[] slots;

    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");

        inventory = player.GetComponent<Inventory>();
    }

    private void OnEnable()
    {
        inventory.OnItemChanged += RefreshUI;
        RefreshUI(0, 0);
    }

    private void OnDisable()
    {
        if (inventory != null)
        {
            inventory.OnItemChanged -= RefreshUI;
        }
    }

    private void RefreshUI(int _, int __)
    {
        foreach (InventorySlotUI slot in slots)
        {
            slot.Clear();
        }

        int slotIndex = 0;

        foreach (var item in inventory.GetAllItems())
        {
            if (slotIndex >= slots.Length)
                break;

            int lootID = item.Key;
            int amount = item.Value;

            LootData loot = LootDatabase.Instance.GetLoot(lootID);

            GameObject prefab = Resources.Load<GameObject>($"Prefabs/Loot/{loot.prefab}");

            Sprite sprite = prefab.GetComponent<SpriteRenderer>().sprite;

            slots[slotIndex].SetItem(sprite, loot.lootName, amount, lootID);

            slotIndex++;
        }
    }

    public int GetLootIDInSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length)
            return -1;

        return slots[slotIndex].CurrentLootID;
    }
}