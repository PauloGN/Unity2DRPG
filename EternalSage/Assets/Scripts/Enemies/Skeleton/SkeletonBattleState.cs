using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    Transform player;
    Skeleton skEnemy;
    int moveDirection;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _StateMachine, string _animBoolName, Skeleton skEnemy) : base(_enemyBase, _StateMachine, _animBoolName)
    {
        this.skEnemy = skEnemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = GameObject.Find("Player").transform;
        stateTimer = skEnemy.battleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (skEnemy.IsPlayerDetected())
        {
            //State timer is aways being decreased in EnemyState class //-> stateTimer -= Time.deltaTime;
            stateTimer = skEnemy.battleTime;

            if (skEnemy.IsPlayerDetected().distance < skEnemy.attackDistance)
            {
                skEnemy.SetZeroVelocity();
                if (CanAttack())
                {
                    fsm.ChangeState(skEnemy.skAttackState);
                }
                return;
            }
        }
        else
        {
            if(stateTimer <= 0.0f || Vector2.Distance(player.transform.position, skEnemy.transform.position) > skEnemy.distanceToForgetPlayer)
            {
               fsm.ChangeState(skEnemy.skIdleState);
                return;
            }
        }

        if (player.position.x > skEnemy.transform.position.x)//move Right
        {
            moveDirection = 1;
        }
        else if (player.position.x < skEnemy.transform.position.x)//move left
        {
            moveDirection = -1;
        }

        skEnemy.SetVelocity(skEnemy.moveSpeed * moveDirection, skEnemy.rb.velocity.y);
    }


    private bool CanAttack()
    {
        if (Time.time >= skEnemy.lastTimeAttacked + skEnemy.attackCooldown)
        {
            skEnemy.lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }

}