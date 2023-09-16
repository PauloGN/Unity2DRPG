using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimationTriggers : MonoBehaviour
{
    Skeleton Skeleton => GetComponentInParent<Skeleton>();

    private void AnimationTrigger()
    {
        Skeleton.AnimationFinishTrigger();
    }
}
