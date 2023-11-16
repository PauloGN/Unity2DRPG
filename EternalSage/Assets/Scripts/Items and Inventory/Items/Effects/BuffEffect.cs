using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item effect/Buff")]
public class BuffEffect : ItemEffect
{
    [SerializeField] private int buffAmount;
    [SerializeField] private float buffDuration;
    [SerializeField] private Stats_Type buffType;

    private PlayerStats playerStats;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        playerStats.IncreaseStatBy(buffAmount, buffDuration, playerStats.GetStat(buffType));
    }
}

