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

        playerRef.skill.sword.ResetCrosshairPosition();
        playerRef.skill.sword.DotsActive(true);
        playerRef.SetZeroVelocity();

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        playerRef.SetZeroVelocity();
        if (Input.GetButtonUp("AimSw"))
        {
            stateMachineRef.ChangeState(playerRef.idleState);
        }

        Vector2 AimPosition = playerRef.skill.sword.GetCrosshairPos();

        if(playerRef.transform.position.x > AimPosition.x && playerRef.facingDir == 1)
        {
            playerRef.Flip();
        }
        else if (playerRef.transform.position.x < AimPosition.x && playerRef.facingDir == -1)
        {
            playerRef.Flip();

        }

    }
}
