using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player playerRef => GetComponentInParent<Player>();
    private void AnimationTrigger()
    {
        playerRef.AnimationTrigger();
    }
}
