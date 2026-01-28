using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject ,IPoolable
{
    [SerializeField]
    protected SkillData data;

    
    //property

    public SkillData Data => data;

    public void Use(ISkillActive caster)
    {
        Execute(caster);
    }

    public abstract void Execute(ISkillActive caster);
    
    public IEnumerator SkillCooltime()
    {
        WaitForSeconds wait = new WaitForSeconds(data.Cooltime);
        yield return wait;
    }
    public IEnumerator SkillReturnCo(GameObject spawnPrefab)
    {
        yield return new WaitForSeconds(data.Duration);
        //리턴풀해야겠지
        if (spawnPrefab != null)
        {
            PoolManager.instance.PoolDic[PoolType.Skill].ReturnPool(spawnPrefab);
        }
    }

    public IEnumerator SkillDestroyCo(GameObject spawnPrefab)
    {
        yield return new WaitForSeconds(data.Duration);
        //리턴풀해야겠지
        if (spawnPrefab != null)
        {
            Destroy(spawnPrefab);
        }
    }

    public int GetID()
    {
        return data.ID;
    }
}
