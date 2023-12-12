using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{

    public static Inventory instance;

    public List<ItemData> startingItems;

    public List<InventoryItem> equipmentInUse;
    public Dictionary<ItemDataEquipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventoryMatItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;
   
    public List<InventoryItem> equipmentStashItems;
    public Dictionary<ItemData, InventoryItem> EquipmentStashDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    //UI controllers
    private UI_ItemSlot[] inventoryItemSlot;   //Material
    private UI_ItemSlot[] stashItemSlot;       //Equipment
    private UI_EquipmentSlot[] equipmentSlot;  //Equiped
    private UI_SatatSlot[] statSlot;           //stats Info
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
        inventoryMatItems = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        equipmentStashItems = new List<InventoryItem>();
        EquipmentStashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipmentInUse = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemDataEquipment, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        statSlot = statSlotParent.GetComponentsInChildren<UI_SatatSlot>();

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

            if (inventoryMatItems.Count <= 0)
            {
                return;
            }
 
            ItemData itemToRemove = inventoryMatItems[inventoryMatItems.Count -1].data;
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

        equipmentInUse.Add(newItem);
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
            equipmentInUse.Remove(value);
            equipmentDictionary.Remove(_itemToRemove);
            _itemToRemove.RemoveModifiers();
        }
    }

    #endregion

    #region ADD, REMOVE, and UPDATE ITEMS

    public bool CanAddItem()
    {
        if (equipmentStashItems.Count >= stashItemSlot.Length)
        {
            Debug.Log("No more Space");
            return false;
        }
        Debug.Log("Space available: " + (stashItemSlot.Length - (equipmentStashItems.Count + 1)));
        return true;
    }


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

        for (int i = 0; i < inventoryMatItems.Count; ++i)
        {
            inventoryItemSlot[i].UpdateSlot(inventoryMatItems[i], removinglast);///***** inside
        }

        for (int i = 0; i < equipmentStashItems.Count; ++i)
        {
            stashItemSlot[i].UpdateSlot(equipmentStashItems[i], removinglast);
        }

        //Updatting info of stats in character UI
        UpdateStatsInfo();

    }

    public void UpdateStatsInfo()
    {
        for (int i = 0; i < statSlot.Length; ++i)
        {
            statSlot[i].UpdateStatValueUi();
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
                    if (CanAddItem())
                    {
                        AddToStash(_item);
                    }
                }
                break;
            default:
                break;
        }

        UpdateSlotUI();
    }

    private void AddToStash(ItemData _item)
    {
        if (EquipmentStashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            equipmentStashItems.Add(newItem);
            EquipmentStashDictionary.Add(_item, newItem);
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
            inventoryMatItems.Add(newItem);
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
                inventoryMatItems.Remove(value);
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
            if (EquipmentStashDictionary.TryGetValue(_item, out InventoryItem stashValue))
            {
                if (stashValue.stackSize <= 1)
                {
                    equipmentStashItems.Remove(stashValue);
                    EquipmentStashDictionary.Remove(_item);
                }
                else
                {
                    stashValue.RemoveStack();
                }
            }            
        }

        UpdateSlotUI();
    }

    public List<InventoryItem> GetItemList() => inventoryMatItems;

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