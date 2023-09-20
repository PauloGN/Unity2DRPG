using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    { }

    public override void Enter()
    {
        base.Enter();
        playerRef.skill.clone.CreateClone(playerRef.transform);
        stateTimer = playerRef.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();
        playerRef.SetVelocity(0.0f, playerRef.rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (!playerRef.IsGroundDetected() && playerRef.IsWallDetected())
        {
            stateMachineRef.ChangeState(playerRef.wallSlide);
            return;
        }

        playerRef.SetVelocity(playerRef.dashSpeed * playerRef.dashDir, .0f);

        if(stateTimer <= 0.0f)
        {
            stateMachineRef.ChangeState(playerRef.idleState);
        }
    }
}