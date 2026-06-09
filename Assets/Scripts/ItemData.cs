using UnityEngine;
using UnityEngine.UI;

public class ItemData : MonoBehaviour
{
    public int itemID;
    public string itemName;
    public Sprite itemImage;
    [TextArea]
    public string itemDescription = "It's something interesting here! :)";
}
