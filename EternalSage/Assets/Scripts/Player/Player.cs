using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

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
    [Space]
    [Header("Collision Sensors")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckDistance;
    [SerializeField] Transform wallCheck;
    [SerializeField] float wallCheckDistance;
    [SerializeField] LayerMask whatIsGround;

    //controllers
    public int facingDir { get; private set; } = 1;
    private bool facingRight = true;

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion

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

    private void Awake ()
    {
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

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
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

    public void AnimationTrigger() => stateMachine.currenState.AnimationFinishTrigger();

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        //Ground
        float xGroundCheck = groundCheck.position.x;
        float yGroundCheck = groundCheck.position.y - groundCheckDistance;
        Gizmos.DrawLine(groundCheck.position, new Vector2(xGroundCheck,yGroundCheck));
        //Walls
        float xwallCheck = wallCheck.position.x + wallCheckDistance * facingDir;
        float ywallCheck = wallCheck.position.y;
        Gizmos.DrawLine(wallCheck.position, new Vector2(xwallCheck, ywallCheck));
    }

    public void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public void FlipController(float xDir)
    {
        if (xDir > 0 && !facingRight)
        {
            Flip();
        }else if (xDir < 0 && facingRight)
        {
            Flip();
        }
    }
}