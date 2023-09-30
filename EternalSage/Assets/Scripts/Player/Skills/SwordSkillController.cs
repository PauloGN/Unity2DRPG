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

            if(Vector2.Distance(transform.position, player.transform.position) <= 0.9f)
            {
                player.CatchSword();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning) { return; }

        anim.SetBool("Rotation", false);

        canRotate = false;
        cd2D.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent = collision.transform;
    }
}