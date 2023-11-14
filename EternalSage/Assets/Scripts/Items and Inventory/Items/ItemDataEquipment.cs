using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Cape
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemDataEquipment : ItemData
{
    public EquipmentType equipmentType;


    public ItemEffect[] itemEffects;
    [Space]
    [Header("Major Stats")]
    public int strength;       // increase damage and crit.power
    public int agility;        // increase evasion and crit.chance
    public int intelligence;   // increase Magic dmg and magic resistance
    public int vitality;       // increase Hp and physic resistence
    [Space]
    [Header("Offensive Stats")]
    public int damage;
    public int critChance;
    public int critPower;       //Default value 150%
    [Space]
    [Header("Defensive Stats")]
    public int health;
    public int armor;
    public int evasion;
    public int magicalResistance;
    [Space]
    [Header("Magic Stats")]
    public int fireDamage;
    public int iceDamage;
    public int metalDamage;


    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        //Major
        playerStats.strength.AddModifiers(strength);
        playerStats.agility.AddModifiers(agility);
        playerStats.intelligence.AddModifiers(intelligence);
        playerStats.vitality.AddModifiers(vitality);
        //Offencive
        playerStats.damage.AddModifiers(damage);
        playerStats.critChance.AddModifiers(critChance);
        playerStats.critPower.AddModifiers(critPower);
        //Defensive
        playerStats.maxHealth.AddModifiers(health);
        playerStats.armor.AddModifiers(armor);
        playerStats.evasion.AddModifiers(evasion);
        playerStats.magicalResistance.AddModifiers(magicalResistance);
        //Magic
        playerStats.fireDamage.AddModifiers(fireDamage);
        playerStats.iceDamage.AddModifiers(iceDamage);
        playerStats.metalDamage.AddModifiers(metalDamage);
    }
    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        //Major
        playerStats.strength.RemoveModifiers(strength);
        playerStats.agility.RemoveModifiers(agility);
        playerStats.intelligence.RemoveModifiers(intelligence);
        playerStats.vitality.RemoveModifiers(vitality);
        //Offencive
        playerStats.damage.RemoveModifiers(damage);
        playerStats.critChance.RemoveModifiers(critChance);
        playerStats.critPower.RemoveModifiers(critPower);
        //Defensive
        playerStats.maxHealth.RemoveModifiers(health);
        playerStats.armor.RemoveModifiers(armor);
        playerStats.evasion.RemoveModifiers(evasion);
        playerStats.magicalResistance.RemoveModifiers(magicalResistance);
        //Magic
        playerStats.fireDamage.RemoveModifiers(fireDamage);
        playerStats.iceDamage.RemoveModifiers(iceDamage);
        playerStats.metalDamage.RemoveModifiers(metalDamage);
    }

    public void Effect(Transform _enemyPosition)
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }
}
