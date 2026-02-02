using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : BossPattern, IHittable
{
    [Header("Totem Hp")]
    [SerializeField]
    private float curHp;
    [SerializeField]
    private float maxHp;

    public bool isDead;
    private Action OnDead;
    public float CurHp
    {
        get
        { return curHp; }
        private set 
        { 
            curHp = value; 
            if(curHp > maxHp)
            {
                maxHp= curHp;
            }
            if(curHp <=0 )
            {
                curHp=0;
                Die();
            }
            
        }
    }

    private void OnEnable()
    {
        curHp = maxHp;
        isDead = false;
        OnDead = null;
    }
    public void Init(Action deathCallback)
    {
        OnDead = deathCallback;
    }
    public void Hit(DamageInfo info)
    {
        if (isDead) return;
        CurHp -= info.damage;
    }
    public void Die()
    {
        isDead= true;
        //리턴해야겠지
        OnDead?.Invoke();
        OnDead = null;
        PoolManager.instance.PoolDic[PoolType.BossPattern].ReturnPool(this);
    }


}
