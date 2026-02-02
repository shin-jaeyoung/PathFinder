using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour, IHittable ,IPoolable
{
    [Header("Totem Hp")]
    private float curHp;
    [SerializeField]
    private float maxHp;

    public bool isDead;
    [Header("PoolData")]
    [SerializeField]
    private int id;
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

    private void Awake()
    {
        curHp = maxHp;
        isDead = false;
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
        Destroy(gameObject);
    }

    public int GetID()
    {
        return id;
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
