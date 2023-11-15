using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item effect/Armor/Freeze Enemies")]
public class FreezeEnemiesEffect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_enemyPosition.position, 2.0f);

        foreach (Collider2D hit in colliders)
        {
            hit.GetComponent<Enemy>()?.FreezeTimeCall(duration);
        }
    }
}
