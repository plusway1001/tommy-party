using UnityEngine;

public class LootPickup : MonoBehaviour
{
    public Loot loot;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = loot.icon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }
        GameManager.instance.AddCurrency(loot.value);
        Destroy(gameObject);
    }
}
