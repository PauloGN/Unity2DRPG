using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{

    #region States

    public SkeletonIdleState skIdleState {get; private set;}
    public SkeletonMoveState skMoveState {get; private set;}

    #endregion


    protected override void Awake()
    {
        base.Awake();

        skIdleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        skMoveState = new SkeletonMoveState(this, stateMachine, "Move", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(skIdleState);
    }

    protected override void Update()
    {
        base.Update();
    }
}
