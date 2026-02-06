using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour , IPoolable
{
    [Header("Projectile ID")]
    [SerializeField]
    private int projectileID;
    public LayerMask obstacle;
    public Rigidbody2D rb;
    private EntityType attackerType;
    private float damage;
    private Entity attacker;
    private bool isHavetoDisapear;

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public int GetID()
    {
        return projectileID;
    }
    private void Awake()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }
    public void Init(float damage, Entity attacker, EntityType attackerType, bool canPass = true)
    {
        this.damage = damage;
        this.attacker = attacker;
        this.attackerType = attackerType;
        isHavetoDisapear = !canPass;
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isHavetoDisapear)
        {
            if(((1<< collision.gameObject.layer)&obstacle.value) !=0)
            {
                if (gameObject != null)
                {
                    PoolManager.instance.PoolDic[PoolType.Skill].ReturnPool(this);
                }
            }
        }

        if(collision.TryGetComponent(out IHittable target))
        {
            
            if(collision.TryGetComponent(out Entity targetEntity))
            {
                
                if (targetEntity.GetEntityType() == attackerType) return;
                DamageInfo finalInfo = new DamageInfo(damage, attacker, targetEntity);
                targetEntity.Hit(finalInfo);
            }
            else
            {
                DamageInfo finalInfo = new DamageInfo(damage, attacker, null);
                target.Hit(finalInfo);
            }
            if (isHavetoDisapear)
            {
                
                if (gameObject != null)
                {
                    PoolManager.instance.PoolDic[PoolType.Skill].ReturnPool(this);
                }
            }
        }
    }
}
