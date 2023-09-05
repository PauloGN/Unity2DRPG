using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;
    private float lastTimeAttacked;
    private float comboWindow = 2;

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

        Debug.Log(comboCounter);
    }

    public override void Exit()
    {
        base.Exit();
        lastTimeAttacked = Time.time;
        comboCounter = (comboCounter + 1)%3;
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            stateMachineRef.ChangeState(playerRef.idleState);
        }
    }
}
