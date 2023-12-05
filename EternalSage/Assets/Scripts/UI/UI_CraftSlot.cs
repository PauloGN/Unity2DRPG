using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{

    protected override void Start()
    {
        base.Start();
    }

    private void OnEnable()
    {
        UpdateSlot(item);
    }

    public void SetupCraftSlot(ItemDataEquipment data)
    {
        if(data == null) { return; }
        item.data = data;

        itemImage.sprite = data.icon;
        itemText.text = data.itemName;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {

        ui.CraftWindow.SetupCraftWindow(item.data as ItemDataEquipment);

        //// inventory craft data
        //ItemDataEquipment craftData = item.data as ItemDataEquipment;

        //if (Inventory.instance.CanCraft(craftData, craftData.craftingMaterials))
        //{
        //    // play sound of FX
        //}
    }
}
