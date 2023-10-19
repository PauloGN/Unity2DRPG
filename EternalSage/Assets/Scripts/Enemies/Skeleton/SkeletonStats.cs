using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStats : EnemyStats
{

    Skeleton skEnemy;

    protected override void Start()
    {
        base.Start();
        skEnemy = GetComponent<Skeleton>();
    }
    public override void TakeDamage(int _dmg)
    {
        base.TakeDamage(_dmg);

    }
    protected override void Die()
    {
        base.Die();
        skEnemy.Die();
    }
}