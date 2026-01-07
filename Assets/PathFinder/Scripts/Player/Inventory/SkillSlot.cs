using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class SkillSlot
{
    public string skill;

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
