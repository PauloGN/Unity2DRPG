using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CrystalSkill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;
    [Header("Cristal info setup")]
    [SerializeField] private bool cloneInsteadOfCrystal;
    [SerializeField] private float crystalDuration = 5.0f;
    [Header("Moving crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;
    [Header("Explosive crystal")]
    [SerializeField] private bool canExplode;
    [Header("Mult stacking crystal")]
    [SerializeField] private bool canUseMultistacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multistaksCoolDown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalsLeft = new List<GameObject>();


    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
        {
            return;
        }

        if (currentCrystal == null)
        {
            CreateCristal();
        }
        else
        {
            if (canMoveToEnemy)
            {
                return;
            }

            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
                return;
            }

            currentCrystal.GetComponent<CrystalSkillController>()?.FinishCristal();
        }
    }

    public void CreateCristal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        CrystalSkillController currentCrystalScript = currentCrystal.GetComponent<CrystalSkillController>();
        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform));
    }

    public void CurrentCrystalChooseRandomTarget()
    {
        currentCrystal.GetComponent<CrystalSkillController>().ChoneRandonEnemy();
    }

    private bool CanUseMultiCrystal()
    {
        if (canUseMultistacks)
        {
            if (crystalsLeft.Count > 0)
            {
                if (crystalsLeft.Count == amountOfStacks)
                {
                    Invoke("ResetAbility", useTimeWindow);
                }

                cooldown = 0;
                //Pick a crystal from the list
                GameObject crystalToSpawn = crystalsLeft[crystalsLeft.Count - 1];
                //make it an instance
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);
                //remove from the list to make sure that it is decreasing
                crystalsLeft.Remove(crystalToSpawn);
                //setup crystal properties and behavior
                newCrystal.GetComponent<CrystalSkillController>()?.
                    SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform));

                //Setting up skill cooldown
                if (crystalsLeft.Count <= 0)
                {
                    cooldown = multistaksCoolDown;
                    RefilCrystal();
                }

                return true;
            }
        }

        return false;
    }

    private void RefilCrystal()
    {
        int amountToAdd = amountOfStacks - crystalsLeft.Count;

        for (int i = 0; i < amountToAdd; i++)
        {
            crystalsLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if(cooldownTimer > 0)
            return;

        cooldownTimer = multistaksCoolDown;
        RefilCrystal();
    }

}
