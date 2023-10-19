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
    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;       //Default value 150%

    //internal controllers
    [SerializeField] private int currentHelth;

    protected virtual void Start()
    {
        currentHelth = maxHealth.GetValue();
        critPower.SetdefaultValue(150);
    }

    public virtual void DoDamage(EntityStats _targetStats)
    {
        //Check evaision 
        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }

        int totalDamage = damage.GetValue() + strength.GetValue();
        //Check Critical damage chance and amount
        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamageOf(totalDamage);
            Debug.Log("CRITICAL DAMAGE: " + totalDamage);
        }

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

    protected virtual void Die()
    {
        Debug.Log("Entity Die aewwwwwww");
    }

    #region Stats Check and Mechanics
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
    private bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if(Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }
    private int CalculateCriticalDamageOf(int _dmg)
    {
        //Getting a percentage of the value
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        float critDamage = _dmg * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }
    #endregion

}
