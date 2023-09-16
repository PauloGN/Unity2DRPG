using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundState
{
    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _StateMachine, string _animBoolName, Skeleton skEnemy) : base(_enemyBase, _StateMachine, _animBoolName, skEnemy)
    {
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
            skEnemy.SetVelocity(0.0f, skEnemy.rb.velocity.y);
            skEnemy.Flip();
            fsm.ChangeState(skEnemy.skIdleState);
        }
    }
}
