using TMPro;
using UnityEngine;

public class SellZone : MonoBehaviour
{
    [SerializeField] private GameObject prompt;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerAbilities player = other.GetComponent<PlayerAbilities>();
            if (player != null)
            {
                player.inSellZone = true;
                prompt.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerAbilities player = other.GetComponent<PlayerAbilities>();
            if (player != null)
            {
                player.inSellZone = false;
                prompt.SetActive(false);
            }
        }
    }
}
