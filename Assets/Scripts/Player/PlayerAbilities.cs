using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerAbilities : MonoBehaviour
{
    InventoryUI inventoryUI;
    Health playerHealth;

    [Header("Bomb")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float spawnDistance = 1f;

    [Header("Potion")]
    [SerializeField] private int healAmt;

    public bool inSellZone = false;

    private void Awake()
    {
        inventoryUI = FindFirstObjectByType<InventoryUI>();
        playerHealth = GetComponent<Health>();
    }

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
        int lootID = inventoryUI.GetLootIDInSlot(slotIndex);

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
            case "Potion":
                UsePotion(lootID); 
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

    private void UsePotion(int lootID)
    {
        if (!Inventory.instance.RemoveItem(lootID, 1))
        {
            return;
        }

        playerHealth.currentHealth = Mathf.Min(playerHealth.currentHealth + healAmt, playerHealth.maxHealth);
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        inventoryUI = FindFirstObjectByType<InventoryUI>();
    }
}