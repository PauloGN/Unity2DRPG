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

        //list of equipments on the character
        List<InventoryItem> CurrentItems = inventory.GetItemList();
        //for each item check if should loose item
        foreach (InventoryItem item in CurrentItems)
        {
            if (Random.Range(0, 100) <= chanceToLooseItems)
            {
                DropItem(item.data);
                inventory.RemoveItem(item.data);
            }
        }
    }

}
