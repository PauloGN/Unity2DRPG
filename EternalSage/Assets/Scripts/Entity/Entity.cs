using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Collision Sensors")]
    public Transform attackCheck;
    public float attackRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackForce;
    [SerializeField] protected float knockbackDuration = .2f;


    //controllers
    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;
    protected bool isKnocked = false;

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fX {get; private set; }
    public SpriteRenderer sr { get; private set; }
    public EntityStats stats { get; private set; }
    #endregion

    protected virtual void Awake()
    {
    

    }
    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        fX = GetComponent<EntityFX>();
        stats = GetComponent<EntityStats>();
    }
    protected virtual void Update()
    {


    }

    public virtual void TakeDamage()
    {
        if(fX != null)
        {
            fX.StartCoroutine("FlashFX");
            StartCoroutine("HitKnockback");
        }
        Debug.Log("Damage...");
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;
        rb.velocity = new Vector2(knockbackForce.x * -facingDir, knockbackForce.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
    }

    #region Collision

    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        //Ground
        float xGroundCheck = groundCheck.position.x;
        float yGroundCheck = groundCheck.position.y - groundCheckDistance;
        Gizmos.DrawLine(groundCheck.position, new Vector2(xGroundCheck, yGroundCheck));
        //Walls
        float xwallCheck = wallCheck.position.x + wallCheckDistance * facingDir;
        float ywallCheck = wallCheck.position.y;
        Gizmos.DrawLine(wallCheck.position, new Vector2(xwallCheck, ywallCheck));
        //WireSphere
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
    }
    #endregion
    #region Velocity
    public void SetZeroVelocity()
    {
        if (isKnocked) { return; }

        rb.velocity = Vector2.zero;
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked) { return; }

        rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }
    #endregion
    #region Flip
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
        }
        else if (xDir < 0 && facingRight)
        {
            Flip();
        }
    }
    #endregion

    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
        {
            sr.color = Color.clear;
        }
        else
        {
            sr.color = Color.white;
        }
    }
}