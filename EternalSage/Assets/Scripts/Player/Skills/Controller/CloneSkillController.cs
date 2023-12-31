using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{

    [SerializeField] private float ColorLoosingSpeed;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackRadius = .8f;

    private Animator anim;
    private float cloneTimer;
    private SpriteRenderer sr;
    private Transform closestEnemy;
    private bool canDuplicateClone;
    private int facingDir = 1;
    private float chanceToDuplicate;
    private Player playerRef;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0.0f) 
        { 
            sr.color = new Color(1.0f, 1.0f, 1.0f, sr.color.a - (Time.deltaTime * ColorLoosingSpeed));
            if (sr.color.a <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetupClone(Transform newTransform, float cloneDuration, bool canAttack, Vector3 offset, Transform _ClosestEnemy, bool _canDuplicateClone, float _chanceToDuplicate)
    {
        transform.position = newTransform.position + offset;
        cloneTimer = cloneDuration;
        canDuplicateClone = _canDuplicateClone;
        chanceToDuplicate = _chanceToDuplicate;
        playerRef = PlayerManager.instance.player;
        if (canAttack)
        {
            anim.SetInteger("AttackNumber", Random.Range(1, 4));
        }

        closestEnemy = _ClosestEnemy;
        FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -1.0f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius);

        foreach (Collider2D collider in colliders)
        {
            var enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                playerRef.stats.DoDamage(enemy.GetComponent<EntityStats>());

                if (canDuplicateClone)
                {
                    if (Random.Range(0, 100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreateClone(enemy.transform, new Vector3(1.0f * facingDir, 0));
                    }
                }
            }
        }
    }


    private void FaceClosestTarget()
    {
        if(closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0.0f, 180.0f, 0.0f);
            }
        }
    }
}