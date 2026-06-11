using UnityEngine;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    public List<UpgradeItem> shopItems = new List<UpgradeItem>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InitialiseDefaultItems();
    }

    void InitialiseDefaultItems()
    {
        shopItems = new List<UpgradeItem>
        {
            new UpgradeItem {
                itemName="Damage Up",   
                description="+5 bullet damage",  
                itemPrice=50, 
                maxLevel=5, 
                upgradeType=UpgradeType.Damage,    
                boostValue=5f    },
            new UpgradeItem { 
                itemName="Rapid Fire",  
                description="-0.05s fire delay", 
                itemPrice=75, 
                maxLevel=4, 
                upgradeType=UpgradeType.FireRate,   
                boostValue=0.05f },
            new UpgradeItem { 
                itemName="Speed Boost", 
                description="+0.5 move speed",   
                itemPrice=40, 
                maxLevel=3, 
                upgradeType=UpgradeType.MoveSpeed,  
                boostValue=0.5f  },
        };
    }

    public bool TryBuyItem(int index)
    {
        UpgradeItem item = shopItems[index];

        if (item.IsMaxLevel)                                    return false;
        if (!item.CanAfford(PlayerDataManager.Instance.coins))  return false;

        PlayerDataManager.Instance.coins -= item.itemPrice;
        PlayerDataManager.Instance.ApplyUpgrade(item);
        item.currentLevel++;
        item.itemPrice = Mathf.RoundToInt(item.itemPrice * 1.5f); // scales up each level
        return true;
    }
}