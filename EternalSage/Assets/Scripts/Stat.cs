using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    //This is a base value for any stat that we want to controll
    [SerializeField] private int baseValue;

    public List<int> modifiers;

    public int GetValue()
    {
        int finalValue = baseValue;

        foreach (int modifier in modifiers)
        {
            finalValue += modifier;
        }

        return finalValue;
    }


    public void AddModifiers(int _modifier)
    {
        modifiers.Add(_modifier);
    }
    public void RemoveModifiers(int _modifier)
    {
        modifiers.RemoveAt(_modifier);
    }
}
