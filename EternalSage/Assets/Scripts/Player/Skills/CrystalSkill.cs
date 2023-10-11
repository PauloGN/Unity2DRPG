using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;
    [Header("Cristal info setup")]
    [SerializeField] private float crystalDuration = 5.0f;
    [Header("Moving crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;
    [Header("Explosive crystal")]
    [SerializeField] private bool canExplode;



    public override void UseSkill()
    {
        base.UseSkill();

        if(currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
            CrystalSkillController  currentCrystalScript = currentCrystal.GetComponent<CrystalSkillController>();
            currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed);
        }
        else
        {
            Vector2 playerPos = player.transform.position;

            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;
            currentCrystal.GetComponent<CrystalSkillController>()?.FinishCristal();
        }

    }

}
