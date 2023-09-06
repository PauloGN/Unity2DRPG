using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;
    private float lastTimeAttacked;
    private float comboWindow = 1.5f;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if(comboCounter > 2 || Time.time > lastTimeAttacked + comboWindow)
        {
            comboCounter = 0;
        }

        playerRef.anim.SetInteger("ComboCounter", comboCounter);

        #region Attack Direction
        float attackDir = playerRef.facingDir;
        if(xInput != 0)
        {
            attackDir = xInput;
        }
        #endregion


        playerRef.SetVelocity(playerRef.attackMovement[comboCounter].x * attackDir, playerRef.attackMovement[comboCounter].y );


        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();

        playerRef.StartCoroutine(playerRef.BusyFor(.15f));

        lastTimeAttacked = Time.time;
        comboCounter = (comboCounter + 1)%3;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0.0f)
        {
            playerRef.ZeroVelocity();
        }

        if (triggerCalled)
        {
            stateMachineRef.ChangeState(playerRef.idleState);
        }
    }
}
