using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Skill owner;
    public LayerMask obstacle;
    public Rigidbody2D rb;
    private EntityType attackerType;
    private float damage;
    private Entity attacker;
    private bool isHavetoDisapear;
    public void Init(float damage, Entity attacker, EntityType attackerType, Skill owner, bool isShot = false)
    {
        this.owner = owner;
        this.damage = damage;
        this.attacker = attacker;
        this.attackerType = attackerType;
        rb = GetComponent<Rigidbody2D>();
        isHavetoDisapear = isShot;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isHavetoDisapear)
        {
            if(((1<< collision.gameObject.layer)&obstacle.value) !=0)
            {
                if (gameObject != null)
                {
                    PoolManager.instance.PoolDic[PoolType.Skill].ReturnPool(gameObject);
                }
            }
        }

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
                        PoolManager.instance.PoolDic[PoolType.Skill].ReturnPool(gameObject);
                    }
                }
            }

            //리턴풀로 바꾸기
        }
    }
}
