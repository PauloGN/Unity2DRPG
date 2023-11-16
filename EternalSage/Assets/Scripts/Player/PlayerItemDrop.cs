using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [SerializeField] private float chanceToLooseItems;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;

        // list of equipments on the character
        List<InventoryItem> currentItems = inventory.GetItemList();
        List<InventoryItem> itemsToRemove = new List<InventoryItem>();

        // for each item check if should lose item
        foreach (InventoryItem item in currentItems)
        {
            if (Random.Range(0, 100) <= chanceToLooseItems)
            {
                DropItem(item.data);
                itemsToRemove.Add(item);
            }
        }

        // remove the items outside of the loop
        foreach (InventoryItem itemToRemove in itemsToRemove)
        {
            inventory.RemoveItem(itemToRemove.data);
        }
    }
}