using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    private GameObject player;

    private Health playerHealth;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI moneyText;

    [SerializeField] private TextMeshProUGUI slot1Text;
    [SerializeField] private TextMeshProUGUI slot2Text;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");

        playerHealth = player.GetComponent<Health>();
    }

    private void Update()
    {
        healthText.text = "Health: " + playerHealth.currentHealth.ToString();
        moneyText.text = "Money: " + GameManager.instance.Currency.ToString();
    }
}
