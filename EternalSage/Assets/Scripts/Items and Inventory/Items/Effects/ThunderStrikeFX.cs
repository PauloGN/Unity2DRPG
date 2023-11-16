using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item effect/ThunderStrike")]
public class ThunderStrikeFX : ItemEffect
{

    [SerializeField] GameObject thunderStrikePrefab;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
       GameObject newthunderStrike = Instantiate(thunderStrikePrefab, _enemyPosition.position, Quaternion.identity);
       Destroy(newthunderStrike, .5f);
    }
}
