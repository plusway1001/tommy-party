using UnityEngine;

public class LootPickup : MonoBehaviour
{
    public int lootID;
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Inventory inventory = collision.GetComponent<Inventory>();

        if (inventory == null)
        {
            return;
        }

        inventory.AddItem(lootID, amount);

        Destroy(gameObject);
    }
}
