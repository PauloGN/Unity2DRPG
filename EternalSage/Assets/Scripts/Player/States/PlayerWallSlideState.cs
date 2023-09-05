using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        if (playerRef.IsGroundDetected() || !playerRef.IsWallDetected())
        {
            stateMachineRef.ChangeState(playerRef.idleState);
        }

        if(xInput != 0.0f && playerRef.facingDir != xInput)
        {
            stateMachineRef.ChangeState(playerRef.idleState);
        }

        if(yInput < 0)
        {
            playerRef.SetVelocity(0.0f, playerRef.rb.velocity.y);
        }
        else if(yInput >= 0)
        {
            playerRef.SetVelocity(0.0f, playerRef.rb.velocity.y * playerRef.slideControl);
        }
    }
}
