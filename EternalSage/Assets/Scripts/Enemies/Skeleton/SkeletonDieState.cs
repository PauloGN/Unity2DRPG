using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDieState : EnemyState
{
    Skeleton skEnemy;

    public SkeletonDieState(Enemy _enemyBase, EnemyStateMachine _StateMachine, string _animBoolName, Skeleton _skeleton) : base(_enemyBase, _StateMachine, _animBoolName)
    {
        skEnemy = _skeleton;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        skEnemy.SetZeroVelocity();
        skEnemy.rb.bodyType = RigidbodyType2D.Static;
        skEnemy.cd.enabled = false;   
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        //triggered on the animation time line called by a function
        if (triggerCalled)
        {
            skEnemy.DestroyEnemyObjAt(1.0f);
        }
    }
}
