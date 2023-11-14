using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item effect/IceAndFire")]
public class IceAndFireEffect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private float xVelocity;
    [SerializeField] private float timeToDestroy = 4.0f;


    public override void ExecuteEffect(Transform _respawnPosition)
    {
        //Spawn the object always looking at the same as the player facing direction
        Player playerRef = PlayerManager.instance.player;
        bool thirdAttac = (playerRef.primaryAttack.comboCounter == 2);

        if (thirdAttac)
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _respawnPosition.position, playerRef.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * playerRef.facingDir, 0.0f);
            Destroy(newIceAndFire, timeToDestroy);
        }
    }

}
