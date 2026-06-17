using UnityEngine;
using UnityEngine.UI;

public class ItemData : MonoBehaviour
{
    public int ItemIDNumber;
    public int itemID;
    public string itemName;
    public Sprite itemImage;
    public string itemDescription = "It's something interesting here! :)";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadCSVInventItemData();
    }

    public void LoadCSVInventItemData()
    {
        IDdata data = ItemDatabase.Instance.GetItem(ItemIDNumber);

        itemID = data.itemID;
        itemName = data.itemName;
        itemImage = data.itemImage;
        itemDescription = data.itemDescription;

        Debug.Log(data.itemID);
        Debug.Log(data.itemName);
        Debug.Log(data.itemImage);
        Debug.Log(data.itemDescription);
    }
}
