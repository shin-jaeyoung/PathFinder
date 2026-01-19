using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity
{
    [SerializeField]
    private MonsterData data;
    [SerializeField]
    private List<Skill> skills;


    public override void Active(int index)
    {
        
    }

    public override void Hit(DamageInfo info)
    {
        
    }
}
