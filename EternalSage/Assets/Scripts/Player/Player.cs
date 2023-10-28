using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : Entity
{
    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttackdurationTime = 0.2f;

    [Header("Move info")]
    public float moveSpeed = 12.0f;
    public float jumpForce = 12.0f;
    public float xJumpForce = 6.0f;
    private float defaultMoveSpeed;
    private float defaultJumpForce;
    [Range(0.0f, 1.0f)]
    public float airControl = 0.5f;
    [Range(0.0f, 1.0f)]
    public float slideControl = 0.5f;
    public float swordReturnImpact = 8.0f;
    [Header("Dash info")]
    [Space]
    public float dashSpeed = 25.0f;
    public float dashDuration = .5f;
    private float defaultDashSpeed;
    public float dashDir { get; private set; } = 1;

    //controllers
    public bool isBusy { get; private set; } = false;

    #region FSM System
    public PlayerStateMachine stateMachine
    {
        get; private set;
    }
    public PlayerIdleState idleState
    {
        get; private set;
    }
    public PlayerMoveState moveState
    {
        get; private set;
    }
    public PlayerJumpState jumpState
    {
        get; private set;
    }
    public PlayerAirState airState
    {
        get; private set;
    }
    public PlayerWallSlideState wallSlide
    {
        get; private set;
    }
    public PlayerDashState dashState
    {
        get; private set;
    }
    public PlayerWallJumpState wallJump
    {
        get; private set;
    }
    public PlayerDeadState deadState
    {
        get; private set;
    }

    //Attack
    public PlayerPrimaryAttackState primaryAttack
    {
        get; private set;
    }
    public PlayerCounterAttackState counterAttack
    {
        get; private set;
    }
    public PlayerAimSwordState aimSword
    {
        get; private set;
    }
    public PlayerCatchSwordState catchSword
    {
        get; private set;
    }
    public PlayerBlackholeState blackhole
    {
        get; private set;
    }

    #endregion

    private PlayerInput playerInput;

    #region Components

    public SkillManager skill
    {
        get; private set;
    }
    public GameObject sword
    {
        get; private set;
    }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState(this, stateMachine, "Die");

        //Attack
        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        aimSword = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSword = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackhole = new PlayerBlackholeState(this, stateMachine, "Jump");

        //components
        playerInput = GetComponent<PlayerInput>();
    }

    protected override void Start()
    {
        base.Start();
        skill = SkillManager.instance;
        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;

    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currenState.Update();
        //Inputs
        CheckInputForDash();
        if (Input.GetKeyDown(KeyCode.F))
        {
            skill.crystalSkill.CanUseSkill();
        }
        //Debug
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    private void CheckInputForDash()
    {
        if (IsWallDetected()) return;

        if (Input.GetButtonDown("Dash") && SkillManager.instance.dash.CanUseSkill())
        {

            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
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

    //Assign and clear sword
    public void AssignNewSword(GameObject newSword)
    {
        sword = newSword;
    }

    public void CatchSword()
    {
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    public void UltimateSkill()
    {
        Debug.Log("AEWWWWWWWWWWW");
        if (IsGroundDetected())
        {
            stateMachine.ChangeState(blackhole);
        }
    }
}