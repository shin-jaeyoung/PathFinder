using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Magic", menuName = "Skill/SkillType/Magic")]
public class Skill_Magic : Skill
{
    [SerializeField]
    private float spawnDelay;
    [SerializeField]
    private bool isExplosion;
    [SerializeField]
    private GameObject explosionEffectPrefab;
    public override void Execute(ISkillActive caster)
    {
        
    }
}
