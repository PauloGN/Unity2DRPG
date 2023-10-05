using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{

    [SerializeField] private GameObject hotkeyPrefab;
    [SerializeField] private List<KeyCode> KeyCodeList;

    public float maxSize;
    public float growSpeed;
    public bool canGrow;

    //Enemies info
    private List<Transform> targets = new List<Transform>();

    private void Update()
    {
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            //stops all enemies with in the range
            enemy.FreezeTime(true);

            if (KeyCodeList == null || KeyCodeList.Count <= 0)
            {
                Debug.LogWarning("KeyCodeList is zero or null");
                return;
            }
            CreateHotKey(enemy);
        }
    }

    private void CreateHotKey(Enemy enemy)
    {
        //spawns the hotkey to be pressed over the enemy's head
        GameObject newHotKey = Instantiate(hotkeyPrefab, enemy.transform.position + new Vector3(0.0f, 2.0f), Quaternion.identity);

        KeyCode chosenKey = KeyCodeList[Random.Range(0, KeyCodeList.Count)];
        KeyCodeList.Remove(chosenKey);

        BlackHoleHotKeyController newHotKeyScript = newHotKey.GetComponent<BlackHoleHotKeyController>();
        newHotKeyScript.SetupHotKey(chosenKey, enemy.transform, this);
    }

    public void AddEnemyToList(Transform _enemy)=>targets.Add(_enemy);


}
