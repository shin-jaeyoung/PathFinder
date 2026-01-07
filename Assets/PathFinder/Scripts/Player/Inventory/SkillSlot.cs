using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class SkillSlot
{
    public Skill skill;

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
    }
}
