using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    [SerializeField] private EntityStats targetStats;
    [SerializeField] private float speed;
    private int damage;
    private Animator anim;
    private bool triggered;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void SetupThunderStrike(int _damage, EntityStats _stats)
    {
        damage = _damage;
        targetStats = _stats;
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetStats)
        {
            anim.SetTrigger("Hit");
            Destroy(gameObject, .4f);
            return;
        }
        if (triggered) { return; }
        

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;
        if (Vector2.Distance(transform.position, targetStats.transform.position) <= 0.5f)
        {
            anim.transform.localRotation = Quaternion.identity;
            anim.transform.localPosition = new Vector3(0.0f, .35f);
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(2, 2, 2);

            triggered = true;
            anim.SetTrigger("Hit");
            Invoke("DamageAndSelfDestroy", .2f);
        }
    }

    private void DamageAndSelfDestroy()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(damage);
        Destroy(gameObject, .4f);
    }
}
