using UnityEngine;

[System.Serializable]
public class UpgradeItem
{
    public string itemName;
    public string description;
    public int itemPrice;
    public int maxLevel;
    public int currentLevel;
    public UpgradeType upgradeType;
    public float boostValue;

    public bool IsMaxLevel => currentLevel >= maxLevel;
    public bool CanAfford(int coins) => coins >= itemPrice;
}

public enum UpgradeType
{
    Damage,
    FireRate,
    MoveSpeed
}