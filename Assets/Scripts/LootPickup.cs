using UnityEngine;

public class LootPickup : MonoBehaviour
{
    [HideInInspector] public int value;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }
        GameManager.instance.AddCurrency(value);
        Destroy(gameObject);
    }
}
