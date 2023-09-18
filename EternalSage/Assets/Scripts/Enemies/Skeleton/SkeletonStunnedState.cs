using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{

    Skeleton skEnemy;

    public SkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _StateMachine, string _animBoolName, Skeleton skEnemy) : base(_enemyBase, _StateMachine, _animBoolName)
    {
        this.skEnemy = skEnemy;
    }

    public override void Enter()
    {
        base.Enter();

        skEnemy.fX.InvokeRepeating("RedColorBlink",0.0f, .1f);

        //how long skeleton will remain into stunned state
        stateTimer = skEnemy.stunDuration;

        skEnemy.rb. velocity = new Vector2(-skEnemy.facingDir * skEnemy.stunDirection.x, skEnemy.stunDirection.y);
    }

    public override void Exit()
    {
        base.Exit();
        skEnemy.fX.Invoke("CancelRedBlink", 0.0f);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer <= 0.0f)
        {
            fsm.ChangeState(skEnemy.skIdleState);
        }
    }
}