using UnityEngine;

[System.Serializable]
public class IDdata : MonoBehaviour
{
    public int itemID;
    public string itemName;
    public Sprite itemImage;
    [TextArea]
    public string itemDescription = "It's something interesting here! :)";

}
