using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                player.TakeDamage();
            }
        }
    }

    //Functions called in animation attack time line
    #region Counter Attack settings
    private void OpenCounterWindow()=> skEnemy.OpenCounterAttackwindow();
    private void CloseCounterWindow()=> skEnemy.CloseCounterAttackwindow();
    #endregion

}
