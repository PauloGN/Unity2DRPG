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

    public void SetupClone(Transform newTransform, float cloneDuration, bool canAttack, Vector3 offset)
    {
        transform.position = newTransform.position + offset;
        cloneTimer = cloneDuration;

        if (canAttack)
        {
            anim.SetInteger("AttackNumber", Random.Range(1, 4));
        }

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
                enemy.TakeDamage();
            }
        }
    }


    private void FaceClosestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 15);  
        float closestDistance = Mathf.Infinity;


        foreach (Collider2D hit in colliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestEnemy = enemy.transform;
                    closestDistance = distanceToEnemy;
                }
            }   
        }

        if(closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                transform.Rotate(0.0f, 180.0f, 0.0f);
            }
        }
    }
}