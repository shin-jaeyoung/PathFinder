using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class SkillSlot
{
    public Skill skill;

    public bool isCooltime;
    public float currentCooltime;
    public bool IsEmpty()
    {
        if (skill == null)
        {
            return true;
        }
        return false;
    }
    public void Clear()
    {
        skill = null;
        isCooltime = false;
    }
    public bool Use(ISkillActive caster)
    {
        if (IsEmpty() || isCooltime) return false;

        skill.Use(caster);
        return true;
    }
}
