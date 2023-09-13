using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Collision Sensors")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    //controllers
    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion

    protected virtual void Awake()
    {
    

    }
    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    protected virtual void Update()
    {


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
    }
    #endregion

    #region Velocity
    public void ZeroVelocity() => rb.velocity = Vector2.zero;

    public void SetVelocity(float xVelocity, float yVelocity)
    {
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
}
