using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{

    public int damage;
    public int maxHealth;


    //internal controllers
    [SerializeField] private int currentHelth;

    private void Start()
    {
        currentHelth = maxHealth;
    }

    public void TakeDamage(int _dmg)
    {
        currentHelth -= _dmg;

        if(currentHelth <= 0)
        {
            currentHelth = 0;
            Die();
        }

    }

    private void Die()
    {
        throw new NotImplementedException();
    }
}
