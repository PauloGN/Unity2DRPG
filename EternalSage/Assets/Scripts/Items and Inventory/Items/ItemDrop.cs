using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int amountOfDropedItems;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();

    [SerializeField] private GameObject dropPrefab;

    public virtual void GenerateDrop()
    {
        for (int i = 0; i < possibleDrop.Length; i++)
        { 
            if (Random.Range(0,100)<= possibleDrop[i].dropChance)
            {
                dropList.Add(possibleDrop[i]);
            }
        }

        for (int i = 0;i < amountOfDropedItems; i++)
        {
            if(dropList.Count <= 0) { break; }

            ItemData randomitem = dropList[Random.Range(0, dropList.Count -1)];
            dropList.Remove(randomitem);
            DropItem(randomitem);
        }

    }

    public virtual void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randonVelocity = new Vector2(Random.Range(-5, 5), Random.Range(5, 10));
        newDrop.GetComponent<ItemObject>()?.SetupItem(_itemData, randonVelocity);
    }
}
