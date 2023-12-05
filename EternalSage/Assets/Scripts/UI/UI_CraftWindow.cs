using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;

    [SerializeField] private Image[] materialImage;

    [SerializeField] private Button craftButton; 

    public void SetupCraftWindow(ItemDataEquipment _data)
    {

        craftButton.onClick.RemoveAllListeners();

        for (int i = 0; i < materialImage.Length; i++)
        {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for (int i = 0;i < _data.craftingMaterials.Count; i++)
        {
            if (_data.craftingMaterials.Count > materialImage.Length)
            {
                Debug.LogWarning("Check aount of slots to hold material to craft on your Crafting image");
            }

            materialImage[i].sprite = _data.craftingMaterials[i].data.icon;
            materialImage[i].color = Color.white;

            TextMeshProUGUI matSlotText = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();

            matSlotText.text = _data.craftingMaterials[i].stackSize.ToString();
            matSlotText.color = Color.white;

        }

        itemIcon.sprite = _data.icon;
        itemName.text = _data.itemName;
        itemDescription.text = _data.GetDescription();

        craftButton.onClick.AddListener(()=> Inventory.instance.CanCraft(_data, _data.craftingMaterials));
    }
}
