using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public static Inventory instance;

    public List<InventoryItem> equipment;
    public Dictionary<ItemDataEquipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;
   
    public List<InventoryItem> stashItems;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;

    //UI controllers
    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;

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

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemDataEquipment, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot =   equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
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
            UpdateSlotUI();
        }
    }

    #region Equipp, Unequipp and Update Equipments

    public void EquipeItem(ItemData _item)
    {
        ItemDataEquipment newEquipment = _item as ItemDataEquipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemDataEquipment oldEquipment = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = item.Key;
            }
        }

        if(oldEquipment != null)
        {
            //Remove from the inventor
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        //Modifiers are aplied
        newEquipment.AddModifiers();

        RemoveItem(_item);

        UpdateSlotUI();
    }

    private void UnequipItem(ItemDataEquipment _itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(_itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(_itemToRemove);
            _itemToRemove.RemoveModifiers();
        }
    }

    #endregion

    #region ADD, REMOVE, and UPDATE ITEMS
    private void UpdateSlotUI(bool removinglast = false)///********
    {
        //equipe to equipment inventory
        for (int i = 0; i < equipmentSlot.Length; ++i)
        {
            foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                {
                    equipmentSlot[i].UpdateSlot(item.Value);
                }
            }
        }

        for (int i = 0; i < inventoryItemSlot.Length; ++i)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < stashItemSlot.Length; ++i)
        {
            stashItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < inventoryItems.Count; ++i)
        {
            inventoryItemSlot[i].UpdateSlot(inventoryItems[i], removinglast);///***** inside
        }

        for (int i = 0; i < stashItems.Count; ++i)
        {
            stashItemSlot[i].UpdateSlot(stashItems[i], removinglast);
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
                UpdateSlotUI(true);//***********************
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