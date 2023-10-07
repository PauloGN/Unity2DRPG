using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkill : Skill
{

    [SerializeField] private GameObject blackholePrefab;
    [Header("Skill info")]
    [SerializeField] private float blackHoleDuration;
    [SerializeField] private float maxsize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private int amountOfAttack;
    [SerializeField] private float clonesAttackCooldown;
    [SerializeField] private float jumpHeight;

    //controller
    BlackholeSkillController currentBlackHole;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        //Spwans the prefab in the world
        GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);

        currentBlackHole = newBlackhole.GetComponent<BlackholeSkillController>();
        currentBlackHole.SettupBlackhole(maxsize, growSpeed, shrinkSpeed, amountOfAttack, clonesAttackCooldown, blackHoleDuration);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public float JumpHeight()=> jumpHeight;

    public bool BlackHoleFinished()
    {
        if (!currentBlackHole)
        {
            return false;
        }

        if (currentBlackHole.playerCanExitState)
        {
            currentBlackHole = null;
            return true;
        }

        return false;
    }
}
