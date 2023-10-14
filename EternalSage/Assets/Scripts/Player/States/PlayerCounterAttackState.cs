using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{

    private bool canCreateClone;

    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        canCreateClone = true;
        triggerCalled = false;
        stateTimer = playerRef.counterAttackdurationTime;
        playerRef.anim.SetBool("SuccessfulCounterAttack", false);
        playerRef.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(playerRef.attackCheck.position, playerRef.attackRadius);
        foreach (Collider2D collider in colliders)
        {
            var enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (enemy.CanBeStunned())
                {
                    stateTimer = 10.0f;// any value biger than 1 it just needs to last more before exiting state
                    playerRef.anim.SetBool("SuccessfulCounterAttack", true);

                    if (canCreateClone)
                    {
                        canCreateClone = false;
                        playerRef.skill.clone.CreateCloneOnCounterAttack(enemy.transform);
                    }
                }
            }
        }

        if (stateTimer <= 0.0f || triggerCalled)
        {
            stateMachineRef.ChangeState(playerRef.idleState);
        }
    }
}