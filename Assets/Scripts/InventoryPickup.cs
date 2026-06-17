using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryPickup : MonoBehaviour
{
    public InventoryManager invent;

    private void OnTriggerStay2D(Collider2D others)
    {
        if (others.CompareTag("Item") && Keyboard.current.spaceKey.isPressed && InventoryManager.EnabledPickup)
        {
            invent.PlayerInventoryHit(others.gameObject);
        }
    }
}
