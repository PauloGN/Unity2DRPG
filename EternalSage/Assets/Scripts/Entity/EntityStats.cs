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
    public Stat magicalResistance;
    [Space]
    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;       //Default value 150%
    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat metalDamage;
    public int intFactor = 3;   //multipplies the intelligence factor

    //Bool controllers
    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;

    //internal controllers
    [SerializeField] private int currentHelth;

    protected virtual void Start()
    {
        currentHelth = maxHealth.GetValue();
        critPower.SetdefaultValue(150);
    }

    public virtual void DoMagicalDamage(EntityStats _targetStats)
    {
        //calculate the total damage to be applied
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _metalDamage = metalDamage.GetValue();
        int totalMagicalDmg = _fireDamage + _iceDamage + _metalDamage + intelligence.GetValue();

        //Calculate the total resistence to deduct from the total magical damage
        totalMagicalDmg = CheckTargetMagicalResistence(_targetStats, totalMagicalDmg);

        //Apply damage on target
        _targetStats.TakeDamage(totalMagicalDmg);


        //aplying elements logic
        if(Mathf.Max(_fireDamage, _iceDamage, _metalDamage) <= 0)
        {
            return;
        }

        bool canApplyIgnite = (_fireDamage > _iceDamage) && (_fireDamage > _metalDamage); 
        bool canApplyChill = (_iceDamage > _fireDamage) && (_iceDamage > _metalDamage);
        bool canApplyShock = (_metalDamage > _iceDamage) && (_metalDamage > _fireDamage);

        while(!canApplyChill && !canApplyIgnite && !canApplyShock)
        {
            if (Random.value < .5f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyElements(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyElements(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < .5f && _metalDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyElements(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }
        _targetStats.ApplyElements(canApplyIgnite, canApplyChill, canApplyShock);

    }

    public void ApplyElements(bool _ignite, bool _chil, bool _shock)
    {
        if (isChilled || isChilled || isShocked)
        {
            return;
        }

        isShocked = _shock;
        isIgnited = _ignite;
        isChilled = _chil;
    }

    public virtual void DoDamage(EntityStats _targetStats)
    {
        //Check evasion 
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
        //_targetStats.TakeDamage(totalDamage);
        DoMagicalDamage(_targetStats);
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
    //Magic
    private int CheckTargetMagicalResistence(EntityStats _targetStats, int totalMagicalDmg)
    {
        totalMagicalDmg -= _targetStats.magicalResistance.GetValue() + (_targetStats.intelligence.GetValue() * intFactor);
        totalMagicalDmg = Mathf.Clamp(totalMagicalDmg, 0, int.MaxValue);
        return totalMagicalDmg;
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