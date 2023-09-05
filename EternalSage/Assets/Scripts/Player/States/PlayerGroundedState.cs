﻿using System.Collections;
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

        CheckJumpInput();
        CheckAttackInput();

        if (!playerRef.IsGroundDetected())
        {
            stateMachineRef.ChangeState(playerRef.airState);
        }
    }

    #region INPUTS
    private void CheckJumpInput()
    {
        if (Input.GetButtonDown("Jump") && playerRef.IsGroundDetected())
        {
            stateMachineRef.ChangeState(playerRef.jumpState);
        }
    }

    private void CheckAttackInput()
    {
        if (Input.GetButtonDown("Attack") && playerRef.IsGroundDetected())
        {
            stateMachineRef.ChangeState(playerRef.primaryAttack);
        }
    }

    #endregion
}



//INPUTS
/*
    Joystick Button 0: X
    Joystick Button 1: O (Circulo)
    Joystick Button 2: Square
    Joystick Button 3: Triangulo
    Joystick Button 4: L1
    Joystick Button 5: R1
    Joystick Button 6: L2 (gatilho esquerdo)
    Joystick Button 7: R2 (gatilho direito)
    Joystick Button 8: Share
    Joystick Button 9: Options
    Joystick Button 10: Analagico Esquerdo (pressionar)
    Joystick Button 11: Analagico Direito (pressionar)
    Joystick Button 12: Touchpad (se pressionado)
    Joystick Button 13: Botao PlayStation (PS)
 */