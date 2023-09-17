using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack details")]
    public Vector2[] attackMovement;

    [Header("Move info")]
    public float moveSpeed = 12.0f;
    public float jumpForce = 12.0f;
    public float xJumpForce = 6.0f;
    [Range(0.0f, 1.0f)]
    public float airControl = 0.5f;
    [Range(0.0f, 1.0f)]
    public float slideControl = 0.5f;
    [Header("Dash info")]
    [Space]
    [SerializeField] float dashCoolDown = 3.0f;
    private float dashTimerUsage = 0.0f;
    public float dashSpeed = 25.0f;
    public float dashDuration = .5f;
    public float dashDir { get; private set; } = 1;

    //controllers
    public bool isBusy { get; private set; } = false;

    #region FSM System
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set;}
    public PlayerAirState airState { get; private set;}
    public PlayerWallSlideState wallSlide { get; private set;}
    public PlayerDashState dashState { get; private set;}
    public PlayerWallJumpState wallJump { get; private set;}

    //Attack
    public PlayerPrimaryAttackState primaryAttack { get; private set;}

    #endregion

    protected override void Awake ()
    {
        base.Awake ();
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState  = new PlayerAirState(this, stateMachine, "Jump");
        dashState  = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");

        //Attack
        primaryAttack = new PlayerPrimaryAttackState (this, stateMachine, "Attack");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currenState.Update();
        //Inputs
        CheckInputForDash();
        //Debug
    }

    private void CheckInputForDash ()
    {
        if (IsWallDetected()) return;

        dashTimerUsage -= Time.deltaTime;

        if (Input.GetButtonDown("Dash") && dashTimerUsage < 0.0f)
        {
            dashTimerUsage = dashCoolDown;
            dashDir = Input.GetAxisRaw("Horizontal");

            if(dashDir == 0)
            {
                dashDir = facingDir;
            }

            stateMachine.ChangeState(dashState);
        }
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currenState.AnimationFinishTrigger();

}