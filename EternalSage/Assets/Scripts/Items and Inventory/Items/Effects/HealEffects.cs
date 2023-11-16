using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item effect/Heal")]
public class HealEffects : ItemEffect
{
    [Range(0.0f, 1.0f)]
    [SerializeField] private float healPercent;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        //Get player stats
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        //decide how much to heal
        int amountToheal = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercent);

        //apply healing 
        playerStats.IncreaseHealthBy(amountToheal);
    }
}
