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

        if (IsHealthLessThan60Percent())
        {
           Inventory.instance.UseFlask();
        }
    }

    public bool IsHealthLessThan60Percent() => (currentHelth <= (GetMaxHealthValue() * .6f));
        
    protected override void Die()
    {
        base.Die();
        player.Die();
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

}
