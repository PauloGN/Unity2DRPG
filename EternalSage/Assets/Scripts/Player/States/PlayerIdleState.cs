using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        playerRef.SetVelocity(0.0f, playerRef.rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (xInput == playerRef.facingDir && playerRef.IsWallDetected())
        {
            return;
        }

        if(xInput != 0 && !playerRef.isBusy)
        {
            stateMachineRef.ChangeState(playerRef.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}