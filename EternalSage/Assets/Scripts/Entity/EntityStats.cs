using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public Stat strenght;
    public Stat damage;
    public Stat maxHealth;


    //internal controllers
    [SerializeField] private int currentHelth;

    protected virtual void Start()
    {
        currentHelth = maxHealth.GetValue();
    }

    public virtual void DoDamage(EntityStats _targetStats)
    {
        int totalDamage = damage.GetValue() + strenght.GetValue();
        _targetStats.TakeDamage(totalDamage);
    }

    public virtual void TakeDamage(int _dmg)
    {
        currentHelth -= _dmg;

        Debug.Log("Damage Taken " + _dmg);

        if(currentHelth <= 0)
        {
            currentHelth = 0;
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log("Die aewwwwwww");
    }
}
