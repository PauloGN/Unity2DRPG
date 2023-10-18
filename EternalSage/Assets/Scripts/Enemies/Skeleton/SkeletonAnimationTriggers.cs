using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SkeletonAnimationTriggers : MonoBehaviour
{
    Skeleton skEnemy => GetComponentInParent<Skeleton>();

    private void AnimationTrigger()
    {
        skEnemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(skEnemy.attackCheck.position, skEnemy.attackRadius);

        foreach (Collider2D collider in colliders)
        {
            var player = collider.GetComponent<Player>();
            if (player != null)
            {
                PlayerStats target = player.GetComponent<PlayerStats>();
                skEnemy.stats.DoDamage(target);
            }
        }
    }

    //Functions called in animation attack time line
    #region Counter Attack settings
    private void OpenCounterWindow()=> skEnemy.OpenCounterAttackwindow();
    private void CloseCounterWindow()=> skEnemy.CloseCounterAttackwindow();
    #endregion
}
