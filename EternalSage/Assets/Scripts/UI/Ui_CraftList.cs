using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ui_CraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlotPrefab;

    [SerializeField] private List<ItemDataEquipment> craftEquipment;
    //[SerializeField] private List<UI_CraftSlot> craftSlots;

    // Start is called before the first frame update
    void Start()
    {
        //AssignCraftSlots();
        transform.parent.GetChild(0).GetComponent<Ui_CraftList>().SetupCraftList();
        SettupDefaultCraftWindow();
    }

    //private void AssignCraftSlots()
    //{
    //    for (int i = 0; i < craftSlotParent.childCount; i++)
    //    {
    //        craftSlots.Add(craftSlotParent.GetChild(i).GetComponent<UI_CraftSlot>());
    //    }
    //}

    public void SetupCraftList()
    {
        //for (int i = 0; i < craftSlots.Count; i++)
        //{
        //    Destroy(craftSlots[i].gameObject);
        //}

        //craftSlots = new List<UI_CraftSlot> ();

        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < craftEquipment.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<UI_CraftSlot>().SetupCraftSlot(craftEquipment[i]);
         //   craftSlots.Add(newSlot.GetComponent<UI_CraftSlot>());
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
    }

    public void SettupDefaultCraftWindow()
    {
        if (craftEquipment[0] != null)
        {
            GetComponentInParent<UI>().CraftWindow.SetupCraftWindow(craftEquipment[0]);
        }
    }

}
