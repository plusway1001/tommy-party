using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public int CurrentLootID { get; private set; } = -1;

    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text amountText;

    private void Awake()
    {
        Clear();
    }

    public void SetItem(Sprite sprite, string name, int amount, int lootID)
    {
        CurrentLootID = lootID;

        icon.enabled = true;
        amountText.gameObject.SetActive(true);

        icon.sprite = sprite;
        amountText.text = $"{name}: {amount.ToString()}";
    }

    public void Clear()
    {
        CurrentLootID = -1;

        icon.enabled = false;
        amountText.gameObject.SetActive(false);
    }

    public int GetLootID()
    {
        return CurrentLootID;
    }
}