using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stats_Type
{
    ST_Damage,
    ST_Armor,
    ST_MagicRes,
    ST_Evasion,
    ST_FireDmg,
    ST_MetalDmg,
    ST_IceDmg
}


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
        playerStats.IncreaseStatBy(buffAmount, buffDuration, StatToModify());
    }

    private Stat StatToModify()
    {
        switch (buffType)
        {
            case Stats_Type.ST_Damage:  return playerStats.damage;
            case Stats_Type.ST_Armor:   return playerStats.armor;
            case Stats_Type.ST_MagicRes:return playerStats.magicalResistance;
            case Stats_Type.ST_Evasion: return playerStats.evasion;
            case Stats_Type.ST_FireDmg: return playerStats.fireDamage;
            case Stats_Type.ST_MetalDmg:return playerStats.metalDamage;
            case Stats_Type.ST_IceDmg:  return playerStats.iceDamage;
            default:
            return playerStats.maxHealth;
        }
    }
}

