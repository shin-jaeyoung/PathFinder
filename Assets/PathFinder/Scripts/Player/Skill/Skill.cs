using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject ,ISkillActive<Player>
{
    [SerializeField]
    protected SkillData data;

    protected bool isCooltime;
    
    public void Active(Player caster)
    {
        if (isCooltime) return;
        Execute(caster);
    }

    public abstract void Execute(Player player);

}
