using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_ItemToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    [SerializeField] private int defaultFontSize = 28;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    public void ShowToolTip(ItemDataEquipment _item)
    {
        if(_item == null) { return; }

        itemNameText.text = _item.name;
        itemTypeText.text = _item.equipmentType.ToString();
        itemDescriptionText.text = _item.GetDescription();

        if(itemNameText.text.Length > 12)
        {
            itemNameText.fontSize = defaultFontSize * .7f;
        }
        else
        {
            itemNameText.fontSize = defaultFontSize;
        }

        gameObject.SetActive(true);
    }

    public void HideToolTip() => gameObject.SetActive(false);
}
