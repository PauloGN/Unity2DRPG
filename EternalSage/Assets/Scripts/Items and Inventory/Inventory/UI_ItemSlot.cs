using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    public InventoryItem item;


    public void UpdateSlot(InventoryItem _newItem, bool lastItem = false)
    {
        item = _newItem;

        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.data.icon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                if (lastItem && item.stackSize == 1)
                {
                    ClearImage();
                }
                itemText.text = "";
            }
        }
    }

    public void ClearImage()
    {
        item = null;
        itemImage.color = Color.clear;
        itemImage.sprite = null;
    }

    public void CleanUpSlot()
    {
        item = null;
        itemImage.color = Color.clear;
        itemImage.sprite = null;
        itemText.text = "";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (item.data.itemType == ItemType.Equipment)
        {
            Debug.Log("Equipe new item " + item.data.name);
            Inventory.instance.EquipeItem(item.data);
        }
    }
}

/*
 * enable and use new input system
 https://www.youtube.com/watch?v=Yjee_e4fICc
 https://www.youtube.com/watch?v=Y3WNwl1ObC8&t=296s

 */