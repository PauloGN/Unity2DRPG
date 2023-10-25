using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float flyTime = .4f;
    private bool skillUsed;
    private float defaultGravity;
    private float jumpHeight;

    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        defaultGravity = playerRef.rb.gravityScale;
        skillUsed = false;
        stateTimer = flyTime;
        playerRef.rb.gravityScale = 0f;
        jumpHeight = playerRef.skill.blackhole.JumpHeight();
    }

    public override void Exit()
    {
        base.Exit();
        playerRef.rb.gravityScale = defaultGravity;
        PlayerManager.instance.player.fX.MakeTransparent(false);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0.0f)
        {
            playerRef.rb.velocity = new Vector2(0.0f, jumpHeight);    
        }

        if (stateTimer <= 0.0f)
        {
            playerRef.rb.velocity = new Vector2(0.0f, -.1f);
            if (!skillUsed)
            {
                if (playerRef.skill.blackhole.CanUseSkill())
                {
                    skillUsed = true;
                }
            }
        }

        if (playerRef.skill.blackhole.BlackHoleFinished())
        {
            playerRef.stateMachine.ChangeState(playerRef.airState);
        }

        //NOT IN USE ANY MORE Exit state in blackhole skill controller when all of the attacks are over
        //PlayerManager.instance.player.ExitBlackholeAbility(); line 100
    }
}
