using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    [SerializeField] private GameObject hotkeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;
    [SerializeField] private List<KeyCode> gamePadList;

    public float maxSize;
    public float growSpeed;
    public float shirnkSpeed;
    public bool canGrow;
    public bool canShrink;
    public int amountOfAttacks = 4;
    public float cloneAttackColldown = .3f;

    //Controller
    private bool canCreateHotkeys = true;
    private float cloneAttackTimer = 0.0f;
    private bool cloneAttackReleased;

    //Enemies info
    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotkey = new List<GameObject>(); 


    private void Start()
    {

    }

    private void Update()
    {

        cloneAttackTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R) || Input.GetButton("Attack"))
        {
            DestroyHotKeys();
            cloneAttackReleased = true;
            canCreateHotkeys = false;
        }

        if (cloneAttackTimer <= 0.0f && cloneAttackReleased)
        {
            cloneAttackTimer = cloneAttackColldown;
            int randomIndex = Random.Range(0, targets.Count);

            float Xoffset;

            if (Random.Range(0, 100) > 50)
            {
                Xoffset = 2.0f;
            }
            else
            {
                Xoffset = -2.0f;
            }

            SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(Xoffset, 0.0f));
            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {
                canShrink = true;
                cloneAttackReleased = false;
            }
        }


        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shirnkSpeed * Time.deltaTime);
            if (transform.localScale.x <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void DestroyHotKeys()
    {

        if (createdHotkey.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < createdHotkey.Count; i++)
        {
            Destroy(createdHotkey[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            //stops all enemies with in the range
            enemy.FreezeTime(true);
            CreateHotKey(enemy);
        }
    }

    private void CreateHotKey(Enemy enemy)
    {

        if (keyCodeList == null || keyCodeList.Count <= 0)
        {
            Debug.LogWarning("KeyCodeList is zero or null");
            return;
        }

        if (cloneAttackReleased || !canCreateHotkeys)
        {
            return;
        }

        //spawns the hotkey to be pressed over the enemy's head
        GameObject newHotKey = Instantiate(hotkeyPrefab, enemy.transform.position + new Vector3(0.0f, 2.0f), Quaternion.identity);
        createdHotkey.Add(newHotKey);

        bool joystick = SkillManager.instance.IsJoystickConnected();

        KeyCode chosenKey;
        if (joystick)
        {
            chosenKey = gamePadList[Random.Range(0, gamePadList.Count)];
            gamePadList.Remove(chosenKey);
        }
        else
        {
            chosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];          
            keyCodeList.Remove(chosenKey);
        }

        BlackHoleHotKeyController newHotKeyScript = newHotKey.GetComponent<BlackHoleHotKeyController>();
        newHotKeyScript.SetupHotKey(chosenKey, enemy.transform, this);
    }

    public void AddEnemyToList(Transform _enemy)=>targets.Add(_enemy);
}
