using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;

    [Header("Currency")]
    public int coins = 150;

    [Header("Stats")]
    public float damage    = 10f;
    public float fireRate  = 0.5f;
    public float moveSpeed = 5f;

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    public void ApplyUpgrade(UpgradeItem item)
    {
        switch (item.upgradeType)
        {
            case UpgradeType.Damage:    damage    += item.boostValue; break;
            case UpgradeType.FireRate:  fireRate  -= item.boostValue; break; // lower = faster
            case UpgradeType.MoveSpeed: moveSpeed += item.boostValue; break;
        }
    }
}