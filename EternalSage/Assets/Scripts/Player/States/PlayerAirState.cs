using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        if (playerRef.IsWallDetected())
        {
            stateMachineRef.ChangeState(playerRef.wallSlide);
        }

        if(playerRef.IsGroundDetected())
        {
            stateMachineRef.ChangeState(playerRef.idleState);
        }

        if (xInput != 0.0f)
        {
           float xAirSpeed = xInput * (playerRef.moveSpeed * playerRef.airControl);
           playerRef.SetVelocity(xAirSpeed, playerRef.rb.velocity.y);
        }

    }
}