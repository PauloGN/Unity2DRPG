using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]private Image itemImage;
    [SerializeField]private TextMeshProUGUI itemText;

    public InventoryItem item;


    public void UpdateSlot(InventoryItem _newItem)
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
                itemText.text = "";
            }
        }
    }

    public float cursorSpeed = 5.0f; // Adjust this to control the cursor movement speed.
    public string horizontalAxisName = "RightStickHorizontal"; // Input axis for horizontal movement.
    public string verticalAxisName = "RightStickVertical"; // Input axis for vertical movement.

    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Equipe new item " + item.data.name);
    }
}
