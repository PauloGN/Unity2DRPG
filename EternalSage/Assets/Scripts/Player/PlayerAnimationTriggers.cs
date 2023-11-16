using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player playerRef => GetComponentInParent<Player>();
    private void AnimationTrigger()
    {
        playerRef.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(playerRef.attackCheck.position, playerRef.attackRadius);

        foreach (Collider2D collider in colliders)
        {
            var enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                EnemyStats _target = enemy.GetComponent<EnemyStats>();
                if (_target != null)
                {
                    playerRef.stats.DoDamage(_target);
                }

                //inventory get weapon and call item effect
                ItemDataEquipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);
                if (weaponData != null)
                {
                    weaponData.Effect(_target.transform);
                }
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }

}
