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
    private int igniteDmg;
    [Header("Events")]
    public System.Action onHealthChanged;

    //Bool controllers
    public bool isIgnited;     // Fire damage over time
    public bool isChilled;     // Slow down target reduce armor by 20%
    public bool isShocked;     // reduce accuracy by 20%

    //Timers
    private float ignitedTimer;                 //controller of status duration of ignited effect 
    private float chilledTimer;
    private float shockedTimer;

    private float igniteDamageCoolDown = .3f;   //interval to apply damage when ignited
    private float igniteDmgTimer;               //controller of cooldown


    public int currentHelth;

    protected virtual void Start()
    {
        currentHelth = GetMaxHealthValue();
        critPower.SetdefaultValue(150);
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        igniteDmgTimer -= Time.deltaTime;

        if(ignitedTimer <= 0.0f)
        {
            isIgnited = false;
        }

        if(chilledTimer <= 0.0f)
        {
            isChilled = false;
        }

        if (shockedTimer <= 0.0f)
        {
            isShocked = false;
        }

        if (igniteDmgTimer <= 0 && isIgnited)
        {
            Debug.Log("BURNIIIIIINGGGG!!!!!!!!" + igniteDmg);
            DecreaseHealthBy(igniteDmg);
            if (currentHelth < 0)
            {
                Die();
            }
            igniteDmgTimer = igniteDamageCoolDown;
        }
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

        //aplying elementals logic
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

        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .20f));
        }

        _targetStats.ApplyElements(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void ApplyElements(bool _ignite, bool _chil, bool _shock)
    {
        if (isChilled || isChilled || isShocked)
        {
            return;
        }

        //Duration controller
        if (_ignite)
        {
            isIgnited = _ignite;
            ignitedTimer = 2.0f;
        }

        if (_chil)
        {
            isChilled = _chil;
            chilledTimer = 2.0f;
        }
        if (_shock)
        {
            isShocked = _shock;
            shockedTimer = 2.0f;
        }

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
       DecreaseHealthBy(_dmg);

       Debug.Log("Damage Taken " + _dmg);

        if (currentHelth <= 0)
        {
            currentHelth = 0;
            Die();
        }
    }

    protected virtual void DecreaseHealthBy(int _dmg)
    {
        currentHelth -= _dmg;

        if (onHealthChanged != null)
        {
            onHealthChanged();
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
    public void SetupIgniteDamage(int _dmg) => igniteDmg = _dmg;
    private int CheckTargetArmor(EntityStats _targetStats, int totalDamage)
    {
        //Means if the target being attacke is chilled  its armor is only 80% efficient
        if (_targetStats.isChilled)
        {
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        }
        else
        {
            //not chilled is 100% armor efficient
            totalDamage -= _targetStats.armor.GetValue();
        }

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    private bool TargetCanAvoidAttack(EntityStats _targetStats)
    {
        int totalTargetEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        //Elemental effect
        //means when the entity owner is shocked, it loses 20% of precision
        if (isShocked)
        {
            totalTargetEvasion += 20;
        }

        if (Random.Range(0, 100) < totalTargetEvasion)
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
    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + (vitality.GetValue() * 5);
    }
    #endregion
}