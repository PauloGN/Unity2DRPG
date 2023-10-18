using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{

    #region States

    public SkeletonIdleState skIdleState {get; private set;}
    public SkeletonMoveState skMoveState {get; private set;}
    public SkeletonBattleState skBattleState {get; private set;}
    public SkeletonAttackState skAttackState {get; private set;}
    public SkeletonStunnedState skStunnedState {get; private set;}
    public SkeletonDieState skDieState { get; private set;}

    #endregion


    protected override void Awake()
    {
        base.Awake();

        skIdleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        skMoveState = new SkeletonMoveState(this, stateMachine, "Move", this);
        skBattleState = new SkeletonBattleState(this, stateMachine, "Move", this);
        skAttackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
        skStunnedState = new SkeletonStunnedState(this, stateMachine, "Stunned", this);
        skDieState = new SkeletonDieState(this, stateMachine, "Die", this);
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

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(skStunnedState);
            return true;
        }
        return false;
    }

    public override void Die()
    {
        base.Die();
        CloseCounterAttackwindow();
        stateMachine.ChangeState(skDieState);
    }

}