using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public EnemyStateMachine stateMachine {get; private set;}

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();

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
}
