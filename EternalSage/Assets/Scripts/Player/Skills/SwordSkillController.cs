using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{

    [SerializeField] private float returningSpeed = 12.0f;

    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D cd2D;
    private Player player;


    //controllers
    private bool canRotate = true;
    private bool isReturning;
    private List<Transform> EnemyTargetList;
    //skill bouncing
    [Header("Bounce Info")]
    [SerializeField] private float bounceSpeed = 20.0f;
    private bool isBouncing;
    private int amountOfBounce;
    private int targetIndex;
    //Pierce
    private float pierceAmount;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd2D = GetComponent<Collider2D>();
    }

    public void SetupSword(Vector2 dir, float gravityScale, Player playerIn)
    {
        player = playerIn;
        rb.velocity = dir;
        rb.gravityScale = gravityScale;


        if(pierceAmount <= 0)
            anim.SetBool("Rotation", true);
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        isReturning = true;
       // rb.isKinematic = false;
        transform.parent = null;
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

        BounceLogic();
    }

    private void BounceLogic()
    {
        if (isBouncing && EnemyTargetList.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, EnemyTargetList[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, EnemyTargetList[targetIndex].position) < .10f)
            {
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

        collision.GetComponent<Enemy>()?.TakeDamage();

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

        StuckInto(collision);
    }

    private void StuckInto(Collider2D collision)
    {

        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
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

    public void SetupBounce(bool _isBouncing, int _amountOfBounces)
    {
        EnemyTargetList = new  List<Transform>();

        isBouncing = _isBouncing;
        amountOfBounce = _amountOfBounces;    
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }
}