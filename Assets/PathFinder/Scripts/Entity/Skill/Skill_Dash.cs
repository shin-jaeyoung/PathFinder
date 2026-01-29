using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Dash", menuName = "Skill/Dash")]
public class Skill_Dash : Skill
{
    public override void Execute(ISkillActive caster)
    {
        Debug.Log("dash");
    }

}
