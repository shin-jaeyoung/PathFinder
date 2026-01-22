using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D rb;
    private EntityType attackerType;
    private float damage;
    private Entity attacker;
    private bool isHavetoDisapear;
    public void Init(float damage, Entity attacker, EntityType attackerType, bool isShot = false)
    {
        this.damage = damage;
        this.attacker = attacker;
        this.attackerType = attackerType;
        rb = GetComponent<Rigidbody2D>();
        isHavetoDisapear = isShot;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IHittable target))
        {
            Debug.Log("Hittable");
            if(collision.TryGetComponent(out Entity targetEntity))
            {
                Debug.Log("Entity");
                if (targetEntity.GetEntityType() == attackerType) return;
                DamageInfo finalInfo = new DamageInfo(damage, attacker, targetEntity);
                targetEntity.Hit(finalInfo);
                if(isHavetoDisapear)
                {
                    //리턴풀해야함
                    if (gameObject != null)
                    {
                        Destroy(gameObject);
                    }
                }
            }

            //리턴풀로 바꾸기
        }
    }
}
