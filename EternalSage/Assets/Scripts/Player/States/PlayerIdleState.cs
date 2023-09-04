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
    }

    public override void Update()
    {
        base.Update();
        if(xInput != 0 && stateMachineRef.currenState != playerRef.moveState)
        {
            stateMachineRef.ChangeState(playerRef.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}