using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum pickup_type
{
    keyboard_pickup,
    UIbutton_pickup
}

public class InventoryManager : MonoBehaviour
{
    public int[] storeinventoryID;
    public string[] storeinventoryName;
    public Sprite[] storeinventoryImage;
    public static bool EnabledPickup = true;

    public int[] inventoryID;
    public TextMeshProUGUI[] inventoryName_ID;
    public Image[] inventoryImage;

    public int inventoryloopCount = 0;
    public int inventoryMaxSpace = 3;

    public Button dropinventoryItems, clearswapItemStatus;
    public string[] notesforDroppedItems;
    public Button[] dropItem;
    public int[] dropItemID;
    public GameObject[] dropItemSpawnObject;

    private ItemData item;
    public GameObject[] AllItems;

    public Button[] useitemJanitor, swapInventoryItems;
    public int[] itemforJanitor;
    public bool[] isitemforJanitor;
    public string[] notesforUsedItems;

    public bool[] isswapInventoryItems;
    private int status;

    public Transform Janitor;
    public GameObject[] itemUsedSpawnJanitor;

    public pickup_type pickupType;

    int MouseScrollNum = 0;

    public GameObject[] Selected_Slot;

    private bool isEnabledSwap = false;

    void Start()
    {
        EnabledPickup = true;
        isEnabledSwap = false;

        ClearInventoryData();
        ClearAllUseItemStatus();
        ClearAllSwapItemStatus();

        dropinventoryItems.onClick.AddListener(() => OnDropButtonClicked());

        for (int i = 0; i < dropItem.Length; i++)
        {
            int index = i;
            dropItem[i].onClick.AddListener(() => OnDropButtonClickedV3(index));
            
            useitemJanitor[i].onClick.AddListener(() => UseItemJanitorButtonClicked(index));
            
            swapInventoryItems[i].onClick.AddListener(() => SetSwappableItemStatus(index));
        }
        
        clearswapItemStatus.onClick.AddListener(() => ClearAllSwapItemStatus());
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider == null)
                return;

            // Check tag
            if (hit.collider.CompareTag("Pickup") && pickupType == pickup_type.mouse_pickup)
            {
                //ItemData item = hit.collider.GetComponent<ItemData>();
                item = hit.collider.GetComponent<ItemData>();
                //GetInventoryData(item);
                int inv = inventoryMaxSpace - 1;
                for (int i = 0; i < inventoryMaxSpace; i++)
                {
                    if (isswapInventoryItems[i])
                    {
                        SwapInventoryData();
                        break;
                    }
                    if (i >= inv)
                    {
                        GetInventoryDataV2();
                        break;
                    }
                }

            }
            


        }*/

        CheckUseItemButtonActive();
        CheckSwappableItemStatus();
        SpawnItemUsedJanitor();

        CheckInventorySlotClampValues();

        if (pickupType == pickup_type.keyboard_pickup)
        {
            InputKeyBlindInvent();
        }
    }

    IEnumerator PickupDelay(float duration)
    {
        Debug.Log("Start");
        EnabledPickup = false;

        yield return new WaitForSeconds(duration);

        EnabledPickup = true;
        Debug.Log(duration + "seconds later");
    }

    public void InputKeyBlindInvent()
    {
        float scroll = Mouse.current.scroll.ReadValue().y;

        if (scroll > 0 && MouseScrollNum < 2)
        {
            MouseScrollNum++;
            Debug.Log("Increase: " + MouseScrollNum);
        }
        else if (scroll < 0 && MouseScrollNum > 0)
        {
            MouseScrollNum--;
            Debug.Log("Decrease: " + MouseScrollNum);
        }

        ShowSelectedSlot(MouseScrollNum);
        
        Debug.Log(MouseScrollNum);

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            OnDropButtonClickedV3(MouseScrollNum);
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            UseItemJanitorButtonClickedV2(MouseScrollNum);
        }

        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            isEnabledSwap = !isEnabledSwap;

            Debug.Log("Toggle: " + isEnabledSwap);
        }

        if (isEnabledSwap)
        {
            SetSwappableItemStatus(MouseScrollNum);
        }
        else
        {
            ClearAllSwapItemStatus();
        }
    }

    public void ShowSelectedSlot(int state)
    {
        for(int i = 0; i < Selected_Slot.Length; i++)
        {
            Selected_Slot[i].SetActive(false);
        }

        Selected_Slot[state].SetActive(true);
    }

    public void CheckInventorySlotClampValues()
    {
        inventoryloopCount = Mathf.Clamp(inventoryloopCount, 0, inventoryMaxSpace);
        
        MouseScrollNum = Mathf.Clamp(MouseScrollNum, 0, inventoryMaxSpace - 1);
    }

    public void PlayerInventoryHit(GameObject hit)
    {
            item = hit.GetComponent<ItemData>();

            int inv = inventoryMaxSpace - 1;
            for (int i = 0; i < inventoryMaxSpace; i++)
            {
                if (isswapInventoryItems[i])
                {
                    SwapInventoryData();
                    break;
                }
                if (i >= inv)
                {
                    GetInventoryDataV2();
                    break;
                }
            }
    }
    public void GetInventoryData()
    {
        if (item == null) return;

        if (inventoryloopCount < inventoryMaxSpace)
        {
            for (int i = 0; i < inventoryMaxSpace; i++)
            {
                if (inventoryloopCount == i)
                {
                    inventoryName_ID[i].text = "ID: " + item.itemID + " " + item.itemName;
                    inventoryImage[i].sprite = item.itemImage;
                    
                }
            }
            inventoryloopCount++;
            
            item.gameObject.SetActive(false);
        }
    }

    public void GetInventoryDataV2()
    {
        if (item == null) return;

        if (inventoryloopCount < inventoryMaxSpace)
        {
            for (int i = 0; i < inventoryMaxSpace; i++)
            {
                if (inventoryID[i] == 0)
                {
                    inventoryID[i] = item.itemID;
                    //inventoryName_ID[i].text = "ID: " + item.itemID + " " + item.itemName;
                    inventoryName_ID[i].text = item.itemName;
                    inventoryImage[i].sprite = item.itemImage;
                    break;
                }

            }
            inventoryloopCount++;

            Destroy(item.gameObject);
        }
    }

    public void SwapInventoryData()
    {
        int temp_ID;

        if (item == null) return;

        if (inventoryID[status] == 0) return;

        temp_ID = inventoryID[status];

        inventoryID[status] = item.itemID;
        inventoryName_ID[status].text = item.itemName;
        inventoryImage[status].sprite = item.itemImage;
        //item.gameObject.SetActive(false);
        Destroy(item.gameObject);

        StartCoroutine(PickupDelay(1f));

        for (int i = 0; i < dropItemID.Length; i++)
        {
            if (temp_ID == dropItemID[i])
            {
                Instantiate(dropItemSpawnObject[i], Janitor.position, Janitor.rotation);

            }
        }

        ClearAllSwapItemStatus();
    }

    public void ClearInventoryData()
    {
        inventoryloopCount = 0;
        for (int i = 0; i < inventoryMaxSpace; i++)
        {
            inventoryID[i] = 0;
            inventoryName_ID[i].text = "ID: " + storeinventoryID[i] + " " + storeinventoryName[i];
            inventoryName_ID[i].text = storeinventoryName[i];
            inventoryImage[i].sprite = storeinventoryImage[i];

        }
    }

    void OnDropButtonClicked()
    {
        ClearInventoryData();
        for (int i = 0; i < AllItems.Length; i++)
        {
            AllItems[i].gameObject.SetActive(true);
        }

        
    }

    void OnDropButtonClickedV2(int index)
    {
        if (inventoryloopCount > 0)
        {

            inventoryID[index] = 0;
           
            inventoryName_ID[index].text = storeinventoryName[index];
            inventoryImage[index].sprite = storeinventoryImage[index];



            inventoryloopCount--;
            

        }
    }

    void OnDropButtonClickedV3(int index)
    {
        if (inventoryID[index] == 0) return;

        if (inventoryloopCount > 0)
        {
            for (int i = 0; i < dropItemID.Length; i++)
            {
                if (inventoryID[index] == dropItemID[i])
                {
                    Instantiate(dropItemSpawnObject[i], Janitor.position, Janitor.rotation);
                    
                }
            }
            inventoryID[index] = 0;
           
            inventoryName_ID[index].text = storeinventoryName[index];
            inventoryImage[index].sprite = storeinventoryImage[index];



            inventoryloopCount--;


        }
    }

    void OnDropButtonClickedV3()
    {
        if (inventoryloopCount > 0)
        {

            inventoryID[0] = 0;
            inventoryName_ID[0].text = "ID: " + storeinventoryID[0] + " " + storeinventoryName[0];
            inventoryImage[0].sprite = storeinventoryImage[0];



            inventoryloopCount--;


        }
    }

    public void CheckUseItemButtonActive()
    {
        for (int i = 0; i < inventoryMaxSpace; i++)
        {
            if (inventoryID[i] == 0)
            {

                useitemJanitor[i].gameObject.SetActive(false);
                swapInventoryItems[i].gameObject.SetActive(false);
                dropItem[i].gameObject.SetActive(false);
            }
            else
            {
                
                useitemJanitor[i].gameObject.SetActive(true);
                swapInventoryItems[i].gameObject.SetActive(true);
                dropItem[i].gameObject.SetActive(true);
            }

        }
    }

    public void ClearAllUseItemStatus()
    {
        for (int i = 0; i < isitemforJanitor.Length; i++)
        {

            isitemforJanitor[i] = false;

        }
        

    }

    public void UseItemJanitorButtonClicked(int index)
    {
        if (inventoryloopCount > 0)
        {
            for (int i = 0; i < itemforJanitor.Length; i++)
            {
                if (inventoryID[index] == itemforJanitor[i])
                {
                    isitemforJanitor[i] = true;
                    Debug.Log(isitemforJanitor[i]);
                }
            }
            inventoryID[index] = 0;
            //inventoryName_ID[index].text = "ID: " + storeinventoryID[index] + " " + storeinventoryName[index];
            inventoryName_ID[index].text = storeinventoryName[index];
            inventoryImage[index].sprite = storeinventoryImage[index];



            inventoryloopCount--;


        }
    }

    public void UseItemJanitorButtonClickedV2(int index)
    {
        if (inventoryID[index] == 0) return;

        if (inventoryloopCount > 0)
        {
            for (int i = 0; i < itemforJanitor.Length; i++)
            {
                if (inventoryID[index] == itemforJanitor[i])
                {
                    Instantiate(itemUsedSpawnJanitor[i], Janitor.position, Janitor.rotation);
                    Debug.Log(itemUsedSpawnJanitor[i]);
                }
            }
            inventoryID[index] = 0;
            //inventoryName_ID[index].text = "ID: " + storeinventoryID[index] + " " + storeinventoryName[index];
            inventoryName_ID[index].text = storeinventoryName[index];
            inventoryImage[index].sprite = storeinventoryImage[index];



            inventoryloopCount--;


        }
    }


    public void SpawnItemUsedJanitor()
    {
        for (int i = 0; i < isitemforJanitor.Length; i++)
        {
            if (isitemforJanitor[i] == true)
            {
                Instantiate(itemUsedSpawnJanitor[i], Janitor.position, Janitor.rotation);
                isitemforJanitor[i] = false;
                Debug.Log(isitemforJanitor[i]);
            }
        }
    }


    public void SetSwappableItemStatus(int index)
    {
        status = index;
        ClearAllSwapItemStatus();
        isswapInventoryItems[index] = true;

    }

    public void CheckSwappableItemStatus()
    {
        int inv = inventoryMaxSpace - 1;
        for (int i = 0; i < inventoryMaxSpace; i++)
        {
            if (isswapInventoryItems[i])
            {
                clearswapItemStatus.gameObject.SetActive(true);
                break;
            }
            if (i >= inv)
            {
                clearswapItemStatus.gameObject.SetActive(false);
                break;
            }
        }
    }

    public void ClearAllSwapItemStatus()
    {
        for (int i = 0; i < isswapInventoryItems.Length; i++)
        {

            isswapInventoryItems[i] = false;

        }
    }
}


