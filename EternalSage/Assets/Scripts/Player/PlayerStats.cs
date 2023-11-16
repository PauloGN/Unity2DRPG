using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerStats : EntityStats
{

    private Player player;

    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _dmg)
    {
        base.TakeDamage(_dmg);
        player.DamageInpact();
    }

    public bool IsHealthLessThanXPercent_Default60(float percent= .6f) => (currentHelth <= (GetMaxHealthValue() * .6f));
        
    protected override void Die()
    {
        base.Die();
        player.Die();
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHealthBy(int _dmg)
    {
        base.DecreaseHealthBy(_dmg);

        if (IsHealthLessThanXPercent_Default60())
        {
            Inventory.instance.UseFlask();
        }

        if (IsHealthLessThanXPercent_Default60(.3f))
        {
            //ItemDataEquipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);
            // if(currentArmor != null)
            // {
            //     currentArmor.Effect(player.transform);
            // }

            Inventory.instance.CanUseArmor();
        }
    }

}
