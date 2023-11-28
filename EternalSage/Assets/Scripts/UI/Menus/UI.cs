using UnityEngine;

public class UI : MonoBehaviour
{

    [SerializeField] private GameObject characterUI;

    public UI_ItemToolTip itemToolTip = null;
    [SerializeField] GameObject slotInfo;

    public UI_StatToolTip statToolTip = null;
    [SerializeField] GameObject statInfo;

    [SerializeField] GameObject cursor;
    // Start is called before the first frame update
    void Start()
    {
        itemToolTip = GetComponentInChildren<UI_ItemToolTip>();
        SwitchTo(null);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.C))//character
        //{
        //    SwitchTo(characterUI);
        //}

        //if (Input.GetKeyDown(KeyCode.B)) //Craft
        //{
        //    SwitchTo(characterUI);
        //}

        //if (Input.GetKeyDown(KeyCode.K))//Skill tree
        //{
        //    SwitchTo(characterUI);
        //}

        //if (Input.GetKeyDown(KeyCode.O))//Skill Option
        //{
        //    SwitchTo(characterUI);
        //}
    }

    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            _menu.SetActive(true);
            bool joystick = SkillManager.instance.IsJoystickConnected();
            if (joystick)
            {
                cursor.SetActive(true);
            }
        }
    }

    public void SwitchWithInputTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            cursor.SetActive(false);
            return;
        }
        SwitchTo(_menu);
        
        //bool joystick = SkillManager.instance.IsJoystickConnected();
        //if (joystick)
        //{
        //    cursor.SetActive(true);
        //}
    }

    public void ToggleSlotToolTip(bool isOn)
    {
        slotInfo.SetActive(isOn);
        if (itemToolTip == null)
        {
            itemToolTip = GetComponentInChildren<UI_ItemToolTip>();
        }
    }

    public void ToggleStatToolTip(bool isOn)
    {
        statInfo.SetActive(isOn);
        if (statToolTip == null)
        {
            statToolTip = GetComponentInChildren<UI_StatToolTip>();
        }
    }
}