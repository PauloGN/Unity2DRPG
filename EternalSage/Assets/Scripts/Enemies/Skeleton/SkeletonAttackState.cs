using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{

    Skeleton skEnemy;

    public SkeletonAttackState(Enemy _enemyBase, EnemyStateMachine _StateMachine, string _animBoolName, Skeleton skeleton) : base(_enemyBase, _StateMachine, _animBoolName)
    {
        skEnemy = skeleton;
    }

    public override void Enter()
    {
        base.Enter();

        skEnemy.SetZeroVelocity();

    }

    public override void Exit()
    {
        base.Exit();
        skEnemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
           fsm.ChangeState(skEnemy.skBattleState);
        }

    }
}
