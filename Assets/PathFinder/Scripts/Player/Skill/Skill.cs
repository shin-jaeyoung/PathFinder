using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject 
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

}
