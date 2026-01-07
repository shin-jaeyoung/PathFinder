using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject 
{
    [SerializeField]
    protected SkillData data;

    protected bool isCooltime;
    
    public void Use(ISkillActive caster)
    {
        if (isCooltime) return;
        Execute(caster);
    }

    public abstract void Execute(ISkillActive caster);

}
