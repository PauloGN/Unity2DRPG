using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [Header("Major Stats")]
    public Stat strength;       // increase damage and crit.power
    public Stat agility;        // increase evasion and crit.chance
    public Stat intelligence;   // increase Magic dmg and magic resistance
    public Stat vitality;       // increase Hp and physic resistence
    [Space]
    [Header("Defensive Stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    [Space]
    [Header("Attacking Stats")]
    public Stat damage;

    //internal controllers
    [SerializeField] private int currentHelth;

    protected virtual void Start()
    {
        currentHelth = maxHealth.GetValue();
    }

    public virtual void DoDamage(EntityStats _targetStats)
    {
        //Check evaision 
        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }

        int totalDamage = damage.GetValue() + strength.GetValue();
        //Check Armor power and returns the damage to be aplied
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
    }


    public virtual void TakeDamage(int _dmg)
    {
        currentHelth -= _dmg;

        Debug.Log("Damage Taken " + _dmg);

        if (currentHelth <= 0)
        {
            currentHelth = 0;
            Die();
        }
    }
    private int CheckTargetArmor(EntityStats _targetStats, int totalDamage)
    {
        totalDamage -= _targetStats.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    private bool TargetCanAvoidAttack(EntityStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("Attack avoided");
            return true;
        }
        return false;
    }

    protected virtual void Die()
    {
        Debug.Log("Entity Die aewwwwwww");
    }
}
