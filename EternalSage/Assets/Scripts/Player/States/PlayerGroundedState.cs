using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        if (Input.GetButtonDown("Jump") && playerRef.IsGroundDetected())
        {
            stateMachineRef.ChangeState(playerRef.jumpState);
        }

        if (!playerRef.IsGroundDetected())
        {
            stateMachineRef.ChangeState(playerRef.airState);
        }
    }
}

//INPUTS
/*
    Joystick Button 0: X
    Joystick Button 1: Square
    Joystick Button 2: O (C�rculo)
    Joystick Button 3: Tri�ngulo
    Joystick Button 4: L1
    Joystick Button 5: R1
    Joystick Button 6: L2 (gatilho esquerdo)
    Joystick Button 7: R2 (gatilho direito)
    Joystick Button 8: Share
    Joystick Button 9: Options
    Joystick Button 10: Anal�gico Esquerdo (pressionar)
    Joystick Button 11: Anal�gico Direito (pressionar)
    Joystick Button 12: Touchpad (se pressionado)
    Joystick Button 13: Bot�o PlayStation (PS)
 */