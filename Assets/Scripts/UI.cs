using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Inventory inventory;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI moneyText;

    [SerializeField] private TextMeshProUGUI slot1Text;
    [SerializeField] private TextMeshProUGUI slot2Text;

    private void Update()
    {
        healthText.text = "Health: " + playerHealth.currentHealth.ToString();
        moneyText.text = "Money: " + GameManager.instance.Currency.ToString();
    }
}
