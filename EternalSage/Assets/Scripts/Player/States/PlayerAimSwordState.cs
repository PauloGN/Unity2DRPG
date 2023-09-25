using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        playerRef.skill.sword.DotsActive(true);

    }

    public override void Exit()
    {
        base.Exit();
        playerRef.skill.sword.ResetCrosshairPosition();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetButtonUp("AimSw"))
        {
            stateMachineRef.ChangeState(playerRef.idleState);
        }

    }
}
