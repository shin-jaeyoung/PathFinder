using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EntityType
{
    Player,
    Monster,
}
public abstract class Entity : MonoBehaviour, ISkillActive, IHittable
{
    public abstract float GetAttackPower();
    public abstract EntityType GetEntityType();
    public abstract void Active(int index);
    public abstract void Hit(DamageInfo info);
}
