using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageInfo
{
    public readonly float damage;
    public readonly Entity attacker;
    public readonly Entity target;
    public readonly bool isCritical;
    public DamageInfo(float damage, Entity attacker, Entity target, bool isCritical = false)
    {
        this.damage = damage;
        this.attacker = attacker;
        this.target = target;
        this.isCritical = isCritical;
    }
}
