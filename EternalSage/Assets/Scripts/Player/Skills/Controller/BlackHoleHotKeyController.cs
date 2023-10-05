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
        myText.text = myHotkey.ToString();    
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
}
