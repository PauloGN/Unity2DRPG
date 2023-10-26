using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public static Inventory instance;

    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;
   
    public List<InventoryItem> stashItems;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;


    //UI controllers
    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        inventoryItems = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
       
        stashItems = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (inventoryItems.Count <= 0)
            {
                return;
            }

            ItemData itemToRemove = inventoryItems[inventoryItems.Count -1].data;
            RemoveItem(itemToRemove);
        }
    }


    #region ADD, REMOVE, and UPDATE ITEMS
    private void UpdateSlotUI()
    {
        for (int i = 0; i < inventoryItems.Count; ++i)
        {
            inventoryItemSlot[i].UpdateSlot(inventoryItems[i]);
        }

        for (int i = 0; i < stashItems.Count; ++i)
        {
            stashItemSlot[i].UpdateSlot(stashItems[i]);
        }
    }

    public void AddItem(ItemData _item)
    {

        switch (_item.itemType)
        {
            case ItemType.Material:
                {
                     AddToInventory(_item);
                }
                break;
            case ItemType.Equipment:
                {
                    AddToStash(_item);
                }
                break;
            default:
                break;
        }

        UpdateSlotUI();
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stashItems.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventoryItems.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {

        bool removedItem = false;

        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventoryItems.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
            removedItem = true;
        }

        if (!removedItem)
        {
            if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
            {
                if (stashValue.stackSize <= 1)
                {
                    stashItems.Remove(stashValue);
                    stashDictionary.Remove(_item);
                }
                else
                {
                    stashValue.RemoveStack();
                }
            }            
        }

        UpdateSlotUI();
    }

    #endregion




}
