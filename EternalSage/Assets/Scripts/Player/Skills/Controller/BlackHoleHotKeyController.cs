using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlackHoleHotKeyController : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotkey;
    private TextMeshProUGUI myText;
    private Transform myEnemy;
    private BlackholeSkillController blackhole;

    public void SetupHotKey(KeyCode _myHotkey, Transform _myEnemy, BlackholeSkillController _blackholeSkillController)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();
        myEnemy = _myEnemy;
        blackhole = _blackholeSkillController;

        myHotkey = _myHotkey;

        bool isJoystick = SkillManager.instance.IsJoystickConnected();
        if(isJoystick )
        {
            myText.text = NameOfTheKey(myHotkey);
        }
        else
        {
            myText.text = myHotkey.ToString();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(myHotkey))
        {
            blackhole.AddEnemyToList(myEnemy);
            myText.color = Color.clear;
            sr.color = Color.clear;
            Debug.Log("Hot key is " + myHotkey);
        }
    }

    string NameOfTheKey(KeyCode keyTopress)
    {
        string name = keyTopress.ToString();
        string returnName = "";

        switch (name)
        {
            case "JoystickButton0":
                returnName = "A";
                break;
            case "JoystickButton1":
                returnName = "B";
                break;
            case "JoystickButton3":
                returnName = "Y";
                break;
            case "JoystickButton2":
                returnName = "X";
                break;
            case "JoystickButton4":
                returnName = "LB";
                break;
            case "JoystickButton5":
                returnName = "RB";
                break;
            case "JoystickButton6":
                returnName = "LT";
                break;
            case "JoystickButton7":
                returnName = "RT";
                break;
            default:
                returnName = "X";
                break;

        }

        return returnName;
    }
}
