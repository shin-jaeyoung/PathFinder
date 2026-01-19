using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : Entity
{
    [Header("Data")]
    [SerializeField]
    protected MonsterData data;
    [SerializeField]
    protected List<Skill> skills;

    protected StateMachine<Monster> stateMachine;
    protected float curHp;

    public float CurHp
    {
        get { return curHp; }
        protected set 
        { 
            curHp = value; 
            if(curHp <=0)
            {
                
            }
            if( curHp > data.MaxHp)
            {
                curHp = data.MaxHp;
            }
            OnChangeHp?.Invoke();
        }
    }
    //deligate
    public event Action OnChangeHp;


    private void Start()
    {
        InitStart();
    }
    protected abstract void InitStart();
    public override void Active(int index)
    {
        //스킬 랜덤 발동 함수 따로 만들기
        int randomNum = UnityEngine.Random.Range(0, skills.Count);
        skills[randomNum].Execute(this);
    }

    public override void Hit(DamageInfo info)
    {
        //나중에 세부 계산식이 들어가야함
        CurHp -= info.damage;
    }
}
