using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance { get; set; }

    [SerializeField] private Inventory inventory;
    [SerializeField] private InventorySlotUI[] slots;

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

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void OnEnable()
    {
        inventory.OnItemChanged += RefreshUI;
        RefreshUI(0, 0);
    }

    private void OnDisable()
    {
        inventory.OnItemChanged -= RefreshUI;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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

            slots[slotIndex].SetItem(sprite, amount, lootID);

            slotIndex++;
        }
    }

    public int GetLootIDInSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length)
            return -1;

        return slots[slotIndex].CurrentLootID;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        inventory = FindFirstObjectByType<Inventory>();

        slots = FindObjectsByType<InventorySlotUI>(FindObjectsSortMode.None);

        RefreshUI(0, 0);
    }
}