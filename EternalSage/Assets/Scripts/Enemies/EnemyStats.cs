using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : EntityStats
{

    Enemy enemy;
    [Header("Level details")]
    [SerializeField] private int level = 1;
    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = .4f;

    protected override void Start()
    {
        //Modify enemy properties based on its level
        ApplyLevelModifiers();
        base.Start();
        enemy = GetComponent<Enemy>();

    }

    private void ApplyLevelModifiers()
    {
        //Major
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);
        //Attack
        Modify(damage);
        Modify(critChance);
        Modify(critPower);
        //Defense
        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicalResistance);
        //Elemental
        Modify(fireDamage);
        Modify(iceDamage);
        Modify(metalDamage);
    }

    public override void TakeDamage(int _dmg)
    {
        base.TakeDamage(_dmg);
        enemy.DamageInpact();
    }

    protected void Modify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percentageModifier;
            _stat.AddModifiers(Mathf.RoundToInt(modifier));
        }
    }
}
