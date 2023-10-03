using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{

    private float returningSpeed = 12.0f;
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D cd2D;
    private Player player;

    //controllers
    private bool canRotate = true;
    private bool isReturning;
    private List<Transform> EnemyTargetList;
    private float freezeTimeDuration;
    //skill bouncing
    [Header("Bounce Info")]
    private float bounceSpeed = 20.0f;
    private bool isBouncing;
    private int amountOfBounce;
    private int targetIndex;
    [Header("Pierce Info")]
    //Pierce
    private float pierceAmount;
    //Spin
    [Header("Spin Info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitCooldown;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd2D = GetComponent<Collider2D>();
    }

    public void SetupSword(Vector2 dir, float gravityScale, Player playerIn, float _freezeTimeDuration, float _returnSpeed)
    {
        player = playerIn;
        rb.velocity = dir;
        rb.gravityScale = gravityScale;
        freezeTimeDuration = _freezeTimeDuration;
        returningSpeed = _returnSpeed;

        if (pierceAmount <= 0)
            anim.SetBool("Rotation", true);


        Invoke("DestroyMe", 9.0f);

    }

    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity;
        }

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returningSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) <= 0.9f)
            {
                player.CatchSword();
            }
        }

        SpinLogic();
        BounceLogic();
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        isReturning = true;
       // rb.isKinematic = false;
        transform.parent = null;
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) >= maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;
                if (spinTimer <= 0.0f)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;
                if (hitTimer <= 0.0f)
                {
                    hitTimer = hitCooldown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.0f);
                    foreach (Collider2D hit in colliders)
                    {
                        hit.GetComponent<Enemy>()?.TakeDamage();
                    }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        if (!wasStopped)
        {
            spinTimer = spinDuration;
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            wasStopped = true;
        }
    }

    private void BounceLogic()
    {
        if (isBouncing && EnemyTargetList.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, EnemyTargetList[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, EnemyTargetList[targetIndex].position) < .10f)
            {
                SwordSkillDamage(EnemyTargetList[targetIndex].GetComponent<Enemy>());
                targetIndex++;
                amountOfBounce--;

                if (amountOfBounce <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= EnemyTargetList.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning) { return; }

        //Do not let spinning Freeze enemies
        if (!isSpinning)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy!= null)
            {
                SwordSkillDamage(enemy);
            }
        }

        SetupTargetForBounce(collision);
        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        enemy?.TakeDamage();
        enemy.StartCoroutine("FreezeTimeFor", freezeTimeDuration);
    }

    private void SetupTargetForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && EnemyTargetList.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10.0f);

                foreach (Collider2D hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        EnemyTargetList.Add(hit.transform);
                    }
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {

        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }


        canRotate = false;
        cd2D.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && EnemyTargetList.Count > 0)
        {
            return;
        }
        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBounces, float _bounceSpeed)
    {
        EnemyTargetList = new  List<Transform>();
        bounceSpeed = _bounceSpeed;

        isBouncing = _isBouncing;
        amountOfBounce = _amountOfBounces;    
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetupSpin(bool _isSpinning, float _maxTRravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTRravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }
}