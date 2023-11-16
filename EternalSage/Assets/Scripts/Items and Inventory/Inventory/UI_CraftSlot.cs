using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{

    private void OnEnable()
    {
        UpdateSlot(item);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        // inventory craft data
        ItemDataEquipment craftData = item.data as ItemDataEquipment;

       if(Inventory.instance.CanCraft(craftData, craftData.craftingMaterials))
        {
            // play sound of FX
        }
    }
}
