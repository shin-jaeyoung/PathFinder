using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageInfo
{
    public readonly float damage;
    public readonly Entity attacker;
    public readonly Entity target;

    public DamageInfo(float damage, Entity attacker, Entity target)
    {
        this.damage = damage;
        this.attacker = attacker;
        this.target = target;
    }
}
