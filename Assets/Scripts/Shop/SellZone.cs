using TMPro;
using UnityEngine;

public class SellZone : MonoBehaviour
{
    [SerializeField] private GameObject prompt;
    [SerializeField]
    private AudioClip SellItem;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerAbilities player = other.GetComponent<PlayerAbilities>();
            if (player != null)
            {
                player.inSellZone = true;
                prompt.SetActive(true);
                TriggerSFX.PlayAudioSFX(SellItem);
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
                if (prompt != null)
                {
                    prompt.SetActive(false);
                }
            }
        }
    }
}
