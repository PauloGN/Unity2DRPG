using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{

    private Transform sword;


    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sword = playerRef.sword.transform;

        Vector2 AimPosition = sword.position;
        if (playerRef.transform.position.x > AimPosition.x && playerRef.facingDir == 1)
        {
            playerRef.Flip();
        }
        else if (playerRef.transform.position.x < AimPosition.x && playerRef.facingDir == -1)
        {
            playerRef.Flip();
        }

        playerRef.rb.velocity = new Vector2(playerRef.swordReturnImpact * -playerRef.facingDir, playerRef.rb.velocity.y);

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            stateMachineRef.ChangeState(playerRef.idleState);
        }
    }
}
