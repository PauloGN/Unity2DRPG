using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : EnemyState
{

    Skeleton skEnemy;

    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _StateMachine, string _animBoolName, Skeleton _skEnemy) : base(_enemyBase, _StateMachine, _animBoolName)
    {
        this.skEnemy = _skEnemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = skEnemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0.0f)
        {
            fsm.ChangeState(skEnemy.skMoveState);
        }
    }
}
