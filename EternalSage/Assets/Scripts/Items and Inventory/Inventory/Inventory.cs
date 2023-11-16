using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{

    public static Inventory instance;

    public List<ItemData> startingItems;

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
    //Controllers
    private float lastTimeUsedArmor;

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
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();

        AddStartingItems();

    }

    private void AddStartingItems()
    {
        for (int i = 0; i < startingItems.Count; i++)
        {
            AddItem(startingItems[i]);
        }
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

    public void UnequipItem(ItemDataEquipment _itemToRemove)
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

    public List<InventoryItem> GetItemList() => inventoryItems;

    #endregion

    #region Crafting System

    public bool CanCraft(ItemDataEquipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();

        //Checks the inventory itens to see it can craft or not
        for (int i = 0; i < _requiredMaterials.Count; i++)
        {
            if (inventoryDictionary.TryGetValue(_requiredMaterials[i].data, out InventoryItem stashValue))
            {
                //add to used material
                if(stashValue.stackSize < _requiredMaterials[i].stackSize)
                {
                    //Not enough materials
                    Debug.Log("No enought Mat");
                    return false;
                }
                else
                {
                   materialsToRemove.Add(stashValue);
                }

            }
            else
            {
                //Not enough materials
                Debug.Log("No enought Mat");
                return false;
            }
        }

        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }
        AddItem(_itemToCraft);
        Debug.Log("CRAFT: " + _itemToCraft.name);
        return true;
    }

    #endregion

    #region Item Effects

    public ItemDataEquipment GetEquipment(EquipmentType _equipmentType)
    {
        ItemDataEquipment equipedItem = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _equipmentType)
            {
                equipedItem = item.Key;
            }
        }

        return equipedItem;
    }

    #endregion

    #region USE ITEMS OR EFFECTS FROM THEM
    public void UseFlask()
    {
        ItemDataEquipment equipedItem = GetEquipment(EquipmentType.HealthFlask);

        if (equipedItem != null) 
        { 
            equipedItem.Effect(transform);
            UnequipItem(equipedItem);
            RemoveItem(equipedItem);

            for (int i = 0; i < equipmentSlot.Length; ++i)
            {
               if (equipmentSlot[i].slotType == EquipmentType.HealthFlask)
               {
                   equipmentSlot[i].CleanUpSlot();
               }
            }

        }
    }
    public void UseMagicPosion()
    {
        ItemDataEquipment equipedItem = GetEquipment(EquipmentType.MagicPotion);

        if (equipedItem != null)
        {
            equipedItem.Effect(transform);
            UnequipItem(equipedItem);
            RemoveItem(equipedItem);

            for (int i = 0; i < equipmentSlot.Length; ++i)
            {
                if (equipmentSlot[i].slotType == EquipmentType.MagicPotion)
                {
                    equipmentSlot[i].CleanUpSlot();
                }
            }

        }
    }

    public bool CanUseArmor()
    {
        //Check if there is an armor equipped
        ItemDataEquipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);
        if(currentArmor != null)
        {
            //go over the  time to see if the effect is on cooldown
            if (Time.time > lastTimeUsedArmor + currentArmor.itemEffectsCoolDown)
            {
                lastTimeUsedArmor = Time.time;
                currentArmor.Effect(PlayerManager.instance.player.transform);
                Debug.Log("Armor EFFECT");
                return true;
            }
        }

        Debug.Log("Armor on Cooldown");
        return false;
    }

    #endregion
}