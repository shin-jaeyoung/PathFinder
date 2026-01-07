using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour, ISkillActive, IHittable
{
    public abstract void Active();
    public abstract void Hit(DamageInfo info);
}
