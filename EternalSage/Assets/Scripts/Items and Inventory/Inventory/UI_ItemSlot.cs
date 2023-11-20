using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    public InventoryItem item;
    protected UI ui;

    private void Start()
    {
        if(ui == null)
        {
           ui = GetComponentInParent<UI>();
        }
    }


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

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.data == null) { return; }

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }

        if (item.data.itemType == ItemType.Equipment)
        {
            Debug.Log("Equipe new item " + item.data.name);
            Inventory.instance.EquipeItem(item.data);

            //hide info when equip
            ui.itemToolTip.HideToolTip();

            //chek if the equipped item is a flask
            FlaskLogic();
        }
    }

    private static void FlaskLogic()
    {
        ItemDataEquipment flask = Inventory.instance.GetEquipment(EquipmentType.HealthFlask);
        if (flask != null)
        {
            //When equipping an item see if health is less then 60% if so and it is a flask use it
            PlayerStats stats = (PlayerStats)PlayerManager.instance.player.stats;
            if (stats != null && stats.IsHealthLessThanXPercent_Default60())
            {
                Inventory.instance.UseFlask();
            }
            return;
        }

        //ItemDataEquipment BuffFlask = Inventory.instance.GetEquipment(EquipmentType.MagicPotion);
        //if (flask != BuffFlask)
        //{

        //    Debug.Log("BUUUFFFFF");

        //    return;
        //}
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null || item.data == null) { return; }

        ui.itemToolTip.ShowToolTip(item.data as ItemDataEquipment);

       // throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null || item.data == null) { return; }

        ui.itemToolTip.HideToolTip();

        // throw new System.NotImplementedException();
    }
}

/*
 * enable and use new input system
 https://www.youtube.com/watch?v=Yjee_e4fICc
 https://www.youtube.com/watch?v=Y3WNwl1ObC8&t=296s

 */