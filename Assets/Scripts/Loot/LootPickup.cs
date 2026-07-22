using UnityEngine;

public class LootPickup : MonoBehaviour
{
    public int lootID;
    public int amount = 1;
    [SerializeField] private AudioClip lootSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Inventory inventory = collision.GetComponent<Inventory>();

        if (inventory == null)
        {
            return;
        }

        TriggerSFX.PlayAudioSFX(lootSound);

        inventory.AddItem(lootID, amount);

        Destroy(gameObject);
    }
}
