using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : EnemyState
{
    Skeleton skEnemy;

    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _StateMachine, string _animBoolName, Skeleton _skEnemy) : base(_enemyBase, _StateMachine, _animBoolName)
    {
        this.skEnemy = _skEnemy;

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        skEnemy.SetVelocity(skEnemy.moveSpeed * skEnemy.facingDir, skEnemy.rb.velocity.y);

        if (skEnemy.IsWallDetected() || !skEnemy.IsGroundDetected())
        {
            skEnemy.Flip();
            fsm.ChangeState(skEnemy.skIdleState);
        }
    }
}
