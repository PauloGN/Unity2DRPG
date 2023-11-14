using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_Controller : MonoBehaviour
{
    protected PlayerStats playerStats;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyStats enemyStats = collision.gameObject.GetComponent<EnemyStats>();
        if (enemyStats != null)
        {
            playerStats.DoMagicalDamage(enemyStats);
        }
    }
}
