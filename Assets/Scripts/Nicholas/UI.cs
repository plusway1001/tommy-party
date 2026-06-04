using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Update()
    {
        healthText.text = "Health: " + playerHealth.currentHealth.ToString();
        moneyText.text = "Money: " + GameManager.instance.Currency.ToString();
    }
}
