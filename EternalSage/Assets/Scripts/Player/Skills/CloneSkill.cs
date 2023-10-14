using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class CloneSkill : Skill
{
    [Header("Clone info")]
    [SerializeField] GameObject clonePrefab;
    [SerializeField] float cloneDuration;
    [SerializeField] bool canAttack;

    [SerializeField] bool canCreateCloneOnDashStart;
    [SerializeField] bool canCreateCloneOnDashOver;
    [SerializeField] bool canCreateCloneOnCounterAttack;

    public void CreateClone(Transform clonePosition, Vector3 _offset)
    {
    
        GameObject newClone = Instantiate(clonePrefab);    
    
        newClone.GetComponent<CloneSkillController>().SetupClone(clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(newClone.transform));
    }


    public void CreateCloneOnDashStart()
    {
        if (canCreateCloneOnDashStart)
        {
            CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnDashOver()
    {
        if (canCreateCloneOnDashOver)
        {
            CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnCounterAttack(Transform _target)
    {
        if (canCreateCloneOnCounterAttack)
        {
            StartCoroutine(CreateCloneWithDelay(_target, new Vector3(1.5f * player.facingDir, 0.0f)));
        }
    }


    private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(0.3f);
            CreateClone(_transform, _offset);

    }
}
