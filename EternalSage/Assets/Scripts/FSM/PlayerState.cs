using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    //References
    protected PlayerStateMachine stateMachineRef;
    protected Player playerRef;
    protected float xInput;
    private string animBoolName;

    protected float stateTimer;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
       this.playerRef = _player;
       this.stateMachineRef = _stateMachine;
       this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        playerRef.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        playerRef.anim.SetFloat("yVelocity", playerRef.rb.velocity.y);
        //continually decrease timer
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        playerRef.anim.SetBool(animBoolName, false);
    }
}