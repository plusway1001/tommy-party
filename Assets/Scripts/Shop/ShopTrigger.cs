using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopTrigger : MonoBehaviour
{
    [SerializeField] private ShopItem shopItem;
    [SerializeField] private GameObject promptUI;
    [SerializeField] private TMP_Text promptText;

    private bool playerInRange;

    private void Start()
    {
        promptUI.SetActive(false);

        if (shopItem == null)
        {
            shopItem = GetComponent<ShopItem>();
        }
    }

    private void Update()
    {
        if (!playerInRange)
            return;

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            BuyItem();
        }
    }

    private void BuyItem()
    {
        if (GameManager.instance.Currency < shopItem.price)
        {
            promptText.text = "Not enough coins!";
            return;
        }

        GameManager.instance.AddCurrency(-shopItem.price);

        Inventory.instance.AddItem(shopItem.lootID, shopItem.amount);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = true;

        promptUI.SetActive(true);

        LootData loot = LootDatabase.Instance.GetLoot(shopItem.lootID);
        shopItem.price = loot.buyPrice;

        promptText.text = $"Press E to buy {loot.lootName} ({shopItem.price} coins)";
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = false;

        promptUI.SetActive(false);
    }
}
