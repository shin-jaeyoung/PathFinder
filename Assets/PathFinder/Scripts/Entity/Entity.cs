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

    public abstract DamageInfo GetDamageInfo();
    public abstract EntityType GetEntityType();
    public abstract void Active(int index);
    public abstract void Hit(DamageInfo info);
    public abstract Vector2 LookDir();
    public abstract Vector2 SkillSpawnPos();
    public abstract Vector3 CasterTrasform();
    public abstract Entity GetEntity();
}
