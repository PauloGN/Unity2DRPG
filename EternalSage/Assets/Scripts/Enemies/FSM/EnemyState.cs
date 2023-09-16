using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine fsm;
    protected Enemy enemyBase;
    private string animBoolName;
    
    protected float stateTimer;
    protected bool triggerCalled;

    public EnemyState(Enemy _enemyBase, EnemyStateMachine _StateMachine, string _animBoolName)
    {
        enemyBase = _enemyBase;
        fsm = _StateMachine;
        triggerCalled = false;
        animBoolName = _animBoolName;
    }

    public virtual void Update()
    {

        stateTimer -= Time.deltaTime;

    }


    public virtual void Enter()
    {
        triggerCalled = false;
        enemyBase.anim.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
    }


    public virtual void AnimationFinishTrigger()=>triggerCalled = true;

}
