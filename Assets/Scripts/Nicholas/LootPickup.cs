using UnityEngine;

public class LootPickup : MonoBehaviour
{

    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }
        //GameManager.instance.AddCurrency(loot.value);
        Destroy(gameObject);
    }
}
