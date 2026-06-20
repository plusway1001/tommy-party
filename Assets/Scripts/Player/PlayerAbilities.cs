using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilities : MonoBehaviour
{
    [SerializeField] private GameObject bombPrefab;

    [SerializeField] private float spawnDistance = 1f;

    public bool inSellZone = false;

    private void Update()
    {
        if (Keyboard.current != null)
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                UseSlot(0);
            }

            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                UseSlot(1);
            }
        }
    }

    private void UseSlot(int slotIndex)
    {
        int lootID = InventoryUI.instance.GetLootIDInSlot(slotIndex);

        if (lootID == -1)
        {
            return;
        }

        if (inSellZone)
        {
            SellItem(lootID);
        }
        else
        {
            UseItem(lootID);
        }
    }

    private void UseItem(int lootID)
    {
        LootData loot = LootDatabase.Instance.GetLoot(lootID);

        switch (loot.lootName)
        {
            case "Bomb":
                UseBomb(lootID);
                break;
        }
    }

    private void UseBomb(int lootID)
    {
        if (!Inventory.instance.RemoveItem(lootID, 1))
        {
            return;
        }

        Vector3 spawnPos = transform.position + transform.up * spawnDistance;
        Instantiate(bombPrefab, spawnPos, Quaternion.identity);
    }

    private void SellItem(int lootID)
    {
        LootData loot = LootDatabase.Instance.GetLoot(lootID);

        if (loot == null)
        {
            return;
        }

        if (!Inventory.instance.RemoveItem(lootID, 1))
        {
            return;
        }

        GameManager.instance.AddCurrency(loot.sellPrice);
    }
}