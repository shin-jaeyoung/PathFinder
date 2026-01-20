using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private EntityType attackerType;
    private float damage;
    private Entity attacker;

    public void Init(float damage, Entity attacker, EntityType type)
    {
        this.damage = damage;
        this.attacker = attacker;
        attackerType = type;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IHittable target))
        {
            if(collision.TryGetComponent(out Entity targetEntity))
            {
                if (targetEntity.GetEntityType() == attackerType) return;
                DamageInfo finalInfo = new DamageInfo(damage, attacker, targetEntity);
                targetEntity.Hit(finalInfo);
            }

            //리턴풀
        }
    }
}
