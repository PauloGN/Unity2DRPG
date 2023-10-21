using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : Entity
{
    [Header("Attack Info")]
    public float attackDistance = 1.5f;
    public float attackCooldown = .3f;
    [HideInInspector]public float lastTimeAttacked = 0.0f;
    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    [Header("Sensors info")]
    public float canSeePlayerAtDistance = 50.0f;
    public bool drawFildOfView = true;
    public float distanceToForgetPlayer = 7.0f;
    public float playerDetectionDistance = 2.0f;
    [SerializeField]protected LayerMask whatIsPlayer;
    [Header("Stunned Info")]
    [SerializeField] protected GameObject counterImage;
    public float stunDuration = 1.0f;
    public Vector2 stunDirection = Vector2.zero;
    protected bool canBeStunned;

    public EnemyStateMachine stateMachine {get; private set;}

    //controller
    private float defaultMoveSpeed;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;
    }

    // Start is called before the first frame update
   protected override void Start()
   {
       base.Start();
   }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    //Timer Controller
    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0.0f;
            anim.speed = 0.0f;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1.0f;
        }
    }

    protected virtual IEnumerator FreezeTimeFor(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }

    #region Counter Attack window
    public virtual void OpenCounterAttackwindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackwindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }
    #endregion
    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackwindow();
            return true;
        }

        return false;
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, canSeePlayerAtDistance, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (drawFildOfView)
        {
            //field of view line
            Gizmos.color = Color.green;
            Vector2 from = wallCheck.position;
            Vector2 to = new Vector2(wallCheck.position.x + canSeePlayerAtDistance * facingDir, wallCheck.position.y);
            Gizmos.DrawLine(from, to);

            //Attack line
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + attackDistance * facingDir, transform.position.y));
        }
    }

    public virtual void DestroyEnemyObjAt(float _delayTime = 0.0f)
    {
        Destroy(gameObject, _delayTime);
    }

    //Elemental affect
    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
    }

}