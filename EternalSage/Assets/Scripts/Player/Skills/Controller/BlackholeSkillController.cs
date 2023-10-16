using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    [SerializeField] private GameObject hotkeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;
    [SerializeField] private List<KeyCode> gamePadList;

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private int amountOfAttacks = 4;
    private float cloneAttackColldown = .3f;
    //Time control
    private float blackHoleTimer;

    //Controller
    private bool canCreateHotkeys = true;
    private float cloneAttackTimer = 0.0f;
    private bool cloneAttackReleased;
    private bool canGrow = true;
    private bool canShrink;
    private bool playerCanDesapear = true;

    public bool playerCanExitState { get; private set; }

    //Enemies info
    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotkey = new List<GameObject>(); 


    private void Start()
    {

    }

    private void Update()
    {

        cloneAttackTimer -= Time.deltaTime;
        blackHoleTimer -= Time.deltaTime;

        if(blackHoleTimer < 0.0f)
        {
            blackHoleTimer = Mathf.Infinity;
            if (targets.Count > 0)
            {
                ReleaseCloneAttack();
            }
            else
            {
                FinishBlackholeAbility();
            }
        }

        if (Input.GetKeyDown(KeyCode.R) || Input.GetButton("Attack"))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SettupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown, float _blackHoleDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackColldown = _cloneAttackCooldown;
        blackHoleTimer = _blackHoleDuration;

        if (SkillManager.instance.clone.crystalInsteadOfClone)
        {
            playerCanDesapear = false;
        }
    }

    private void ReleaseCloneAttack()
    {
        if (targets == null || targets.Count <= 0)
        {
            FinishBlackholeAbility();
            Debug.Log("BlackholeSkillController LINE 93");
            return;
        }

        DestroyHotKeys();
        cloneAttackReleased = true;
        canCreateHotkeys = false;

        if (playerCanDesapear)
        {
            playerCanDesapear = false;
            PlayerManager.instance.player.MakeTransparent(true);
        }
    }

    private void CloneAttackLogic()
    {

        if (cloneAttackTimer <= 0.0f && cloneAttackReleased && amountOfAttacks > 0)
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

            if (SkillManager.instance.clone.crystalInsteadOfClone)
            {
                SkillManager.instance.crystalSkill.CreateCristal();
                SkillManager.instance.crystalSkill.CurrentCrystalChooseRandomTarget();
            }
            else
            {
                SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(Xoffset, 0.0f));
            }
            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {
                Invoke("FinishBlackholeAbility", .7f);
            }
        }
    }

    private void FinishBlackholeAbility()
    {
        DestroyHotKeys();
        playerCanExitState = true;
        canShrink = true;
        cloneAttackReleased = false;
        PlayerManager.instance.player.MakeTransparent(false);
    }

    private void DestroyHotKeys()
    {

        if (createdHotkey == null || createdHotkey.Count <= 0)
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            //stops all enemies with in the range
            enemy.FreezeTime(false);
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
