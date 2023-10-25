using System;

[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackSize;

    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        ++stackSize;
    }

    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;

}
