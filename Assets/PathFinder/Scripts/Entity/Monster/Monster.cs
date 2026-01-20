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
    protected List<SkillSlot> skills;
    [Header("Combat")]
    [SerializeField]
    protected  CombatSystem combatSystem;

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
        BaseInit();
        InitStart();
    }
    public void BaseInit()
    {
        combatSystem.Init(this);
        stateMachine = new StateMachine<Monster>(this);
        curHp = data.MaxHp;
    }
    protected abstract void InitStart();
    public override void Active(int index)
    {
        combatSystem.PerformSkill(skills[index]);
        stateMachine.ChangeState(StateType.Attack);
    }

    public override void Hit(DamageInfo info)
    {
        float finalDamage = combatSystem.Hit(info.damage, data.Defence);
        CurHp -= finalDamage;
    }
    public override float GetAttackPower()
    {
        return data.Atk;
    }
    public override EntityType GetEntityType()
    {
        return EntityType.Monster;
    }
}
