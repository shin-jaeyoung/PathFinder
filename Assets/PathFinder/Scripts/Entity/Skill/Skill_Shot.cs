using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Shot", menuName = "Skill/SkillType/Shot")]

public class Skill_Shot : Skill
{
    [SerializeField]
    private float shotSpeed;
    [SerializeField]
    private float count;
    [SerializeField]
    private float deltaShot;
    public override void Execute(ISkillActive caster)
    {
        
    }
}
