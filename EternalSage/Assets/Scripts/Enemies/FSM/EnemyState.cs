using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine fsm;
    protected Enemy enemy;
    private string animBoolName;
    
    protected float stateTimer;
    protected bool triggerCalled;

    public EnemyState(Enemy _enemy, EnemyStateMachine _StateMachine, string _animBoolName)
    {
        enemy = _enemy;
        fsm = _StateMachine;
        triggerCalled = false;
        _animBoolName = animBoolName;
    }

    public virtual void Update()
    {

        stateTimer -= Time.deltaTime;

    }


    public virtual void Enter()
    {
        triggerCalled = false;
        enemy.anim.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        enemy.anim.SetBool(animBoolName, false);
    }

}
