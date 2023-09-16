using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundState : EnemyState
{

    protected Skeleton skEnemy;
    protected Transform player;

    public SkeletonGroundState(Enemy _enemyBase, EnemyStateMachine _StateMachine, string _animBoolName, Skeleton skEnemy) : base(_enemyBase, _StateMachine, _animBoolName)
    {
        this.skEnemy = skEnemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = GameObject.Find("Player").transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (skEnemy.IsPlayerDetected() || Vector2.Distance(skEnemy.transform.position, player.transform.position) < skEnemy.playerDetectionDistance) 
        {
            fsm.ChangeState(skEnemy.skBattleState);
        }
    }
}
