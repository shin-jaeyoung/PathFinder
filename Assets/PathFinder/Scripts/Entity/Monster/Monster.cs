using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : Entity
{
    [Header("Hp")]
    [SerializeField]
    protected float curHp;
    [Header("Data")]
    [SerializeField]
    protected MonsterData data;
    [SerializeField]
    protected List<SkillSlot> skills;
    [Header("Combat")]
    [SerializeField]
    protected  CombatSystem combatSystem;
    [Header("Detection")]
    [SerializeField]
    protected Detection detection;

    protected Rigidbody2D rb;
    protected StateMachine<Monster> stateMachine;

    //property
    public float CurHp
    {
        get { return curHp; }
        protected set 
        { 
            curHp = value; 
            if(curHp <=0)
            {
                curHp = 0;
            }
            if( curHp > data.MaxHp)
            {
                curHp = data.MaxHp;
            }
            OnChangeHp?.Invoke();
        }
    }
    public MonsterData Data => data;
    public Detection Detection => detection;
    public Rigidbody2D Rb => rb;

    //deligate
    public event Action OnChangeHp;


    private void Start()
    {
        BaseInit();
        InitStart();
    }
    public void BaseInit()
    {
        detection = GetComponent<Detection>();
        rb = GetComponent<Rigidbody2D>();
        combatSystem.Init(this);
        stateMachine = new StateMachine<Monster>(this);
        curHp = data.MaxHp;
    }
    protected abstract void InitStart();

    private void Update()
    {
        stateMachine?.Update();
    }
    private void FixedUpdate()
    {
        stateMachine?.FixedUpdate();
    }

    public override void Active(int index)
    {
        combatSystem.PerformSkill(skills[index]);
        stateMachine.ChangeState(StateType.Attack);
    }

    public override void Hit(DamageInfo info)
    {
        float finalDamage = combatSystem.Hit(info.damage, data.Defence);
        CurHp -= finalDamage;
        Debug.Log("몬스터맞음");
    }
    public void FlipSprite(float xInput)
    {
        if (xInput == 0) return;
        float yRotation = (xInput > 0) ? 0f : 180f;
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    public override float GetAttackPower()
    {
        return data.Atk;
    }
    public override EntityType GetEntityType()
    {
        return EntityType.Monster;
    }
    public override Vector2 LookDir()
    {
        return transform.right;
    }
    public override Vector2 SkillSpawnPos()
    {
        return Vector2.zero;
    }
    public override Vector3 CasterTrasform()
    {
        return transform.position;
    }
    public override Entity GetEntity()
    {
        return this;
    }
}
