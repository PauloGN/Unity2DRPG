using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        if (xInput == 0 && stateMachineRef.currenState != playerRef.idleState)
        {
            stateMachineRef.ChangeState(playerRef.idleState);
        }

        playerRef.SetVelocity(xInput * playerRef.moveSpeed, playerRef.rb.velocity.y);

    }
}