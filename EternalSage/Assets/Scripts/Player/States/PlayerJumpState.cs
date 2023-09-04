using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        playerRef.rb.velocity = new Vector2(playerRef.rb.velocity.x, playerRef.jumpForce);

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(playerRef.rb.velocity.y < 0.0f)
        {
            stateMachineRef.ChangeState(playerRef.airState);
        }
    }
}