using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public abstract class Monster : Entity , IPoolable
{
    [Header("Hp")]
    [SerializeField]
    protected float curHp;
    [Header("Data")]
    [SerializeField]
    protected MonsterData data;
    [Header("RewardData")]
    [SerializeField]
    private RewardData rewardData;
    [Header("SKill")]
    [SerializeField]
    protected SkillSlot basicAttack;
    [SerializeField]
    protected List<SkillSlot> skills;
    [Header("Combat")]
    [SerializeField]
    protected CombatSystem combatSystem;
    [Header("Detection")]
    [SerializeField]
    protected Detection detection;

    protected Rigidbody2D rb;
    protected StateMachine<Monster> stateMachine;

    private int useSkillIndex;
    //property
    public float CurHp
    {
        get { return curHp; }
        protected set
        {
            curHp = value;
            if (curHp <= 0)
            {
                curHp = 0;
                Die();
            }
            if (curHp > data.MaxHp)
            {
                curHp = data.MaxHp;
            }
            OnChangeHp?.Invoke();
        }
    }
    public MonsterData Data => data;
    public Detection Detection => detection;
    public Rigidbody2D Rb => rb;
    public SkillSlot BasicAttack => basicAttack;
    public List<SkillSlot> Skills => skills;
    public RewardData RewardData => rewardData;

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
        if (skills.Count > 0 && CheckSkillCool())
        {
            combatSystem.PerformSkill(ActiveNextSkill());
        }
        else
        {
            combatSystem.PerformSkill(basicAttack);
        }
    }
    public bool CheckSkillCool()
    {
        foreach (var skill in skills)
        {
            if (!skill.isCooltime)
            {
                return true;
            }
        }
        return false;
    }
    public SkillSlot ActiveNextSkill()
    {

        while (skills[useSkillIndex].isCooltime)
        {
            ++useSkillIndex;
            if (useSkillIndex >= skills.Count)
            {
                useSkillIndex = 0;
            }
        }
        return skills[useSkillIndex];
    }
    public override void Hit(DamageInfo info)
    {
        float finalDamage = combatSystem.Hit(info.damage, data.Defence);
        CurHp -= finalDamage;
        Debug.Log("몬스터맞음");
    }
    public void Die()
    {
        stateMachine.ChangeState(StateType.Die);
    }
    public void Refresh()
    {
        CurHp = data.MaxHp;
        PoolManager.instance.PoolDic[PoolType.Monster].ReturnPool(gameObject);
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
        Vector2 targetDir = (detection.Target.position - transform.position).normalized;
        return targetDir;
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
    public GameObject GetGO()
    {
        return gameObject;
    }

    public int GetID()
    {
        return data.Id;
    }
}
