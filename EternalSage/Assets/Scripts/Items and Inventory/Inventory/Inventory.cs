using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public static Inventory instance;
    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;
   
    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;


    //UI controllers
    private UI_ItemSlot[] itemSlot;


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

        itemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
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
            itemSlot[i].UpdateSlot(inventoryItems[i]);
        }
    }

    public void AddItem(ItemData _item)
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

        UpdateSlotUI();
    }
    public void RemoveItem(ItemData _item)
    {
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
        }
        UpdateSlotUI();
    }

    #endregion




}
