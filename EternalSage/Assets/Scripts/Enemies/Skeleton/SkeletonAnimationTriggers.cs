using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimationTriggers : MonoBehaviour
{
    Skeleton Skeleton => GetComponentInParent<Skeleton>();

    private void AnimationTrigger()
    {
        Skeleton.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(Skeleton.attackCheck.position, Skeleton.attackRadius);

        foreach (Collider2D collider in colliders)
        {
            var player = collider.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage();
            }
        }
    }
}
