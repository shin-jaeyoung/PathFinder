using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public abstract class Monster : Entity , IPoolable
{
    [System.Serializable]
    public struct MonsterSkillData
    {
        public SkillSlot skill;
        public int animNum;
    }
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
    protected List<MonsterSkillData> skills;
    [Header("Combat")]
    [SerializeField]
    protected CombatSystem combatSystem;
    [Header("Detection")]
    [SerializeField]
    protected Detection detection;

    protected Rigidbody2D rb;
    protected StateMachine<Monster> stateMachine;
    private Animator animator;
    private int animSkillNum;
    [SerializeField]
    protected bool isBerserkerMode;
    protected int useSkillIndex;
    protected bool isDead;

    //property
    public float CurHp
    {
        get { return curHp; }
        protected set
        {
            curHp = value;
            if ((curHp / data.MaxHp) <0.3f)
            {
                
            }
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
    public List<MonsterSkillData> Skills => skills;
    public RewardData RewardData => rewardData;
    public bool IsDead => isDead;
    public int AttackAnimNum => animSkillNum;
    public Animator Animator => animator;
    public bool CanUseAnySkill => skills.Count > 0 && CheckSkillCool();
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
        animator = GetComponent<Animator>();
        combatSystem.Init(this);
        stateMachine = new StateMachine<Monster>(this);
        curHp = data.MaxHp;

        //base상태들
        stateMachine.AddState(StateType.Idle, new MonsterIdleState());
        stateMachine.AddState(StateType.Move, new MonsterMoveState());
        stateMachine.AddState(StateType.Goback, new MonsterGobackState());
        stateMachine.AddState(StateType.Search, new MonsterSearchState());
        stateMachine.AddState(StateType.Attack, new MonsterAttackState());
        stateMachine.AddState(StateType.Hit, new MonsterHitState());
        stateMachine.AddState(StateType.Die, new MonsterDieState());

        stateMachine.ChangeState(StateType.Idle);
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
            MonsterSkillData data = ActiveNextSkill();
            animSkillNum = data.animNum;
            if (combatSystem.PerformSkill(data.skill))
            {
                Debug.Log($"{data.skill.skill.Data.SkillName} 발동");
            }
        }
        else
        {
            animSkillNum = -1;
            if(combatSystem.PerformSkill(basicAttack))
            {
                Debug.Log($"{basicAttack.skill.Data.SkillName} 발동");
            }
        }
    }
    public bool CheckSkillCool()
    {
        foreach (var data in skills)
        {
            if (!data.skill.isCooltime)
            {
                return true;
            }
        }
        return false;
    }
    public MonsterSkillData ActiveNextSkill()
    {

        while (skills[useSkillIndex].skill.isCooltime)
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
        if (stateMachine.CurState == stateMachine.stateDic[StateType.Hit]) return;
        if (stateMachine.CurState == stateMachine.stateDic[StateType.Die]) return;
        if(this is BossMonster)
        {
            if (stateMachine.CurState == stateMachine.stateDic[StateType.Immortal]) return;
        }
        float finalDamage = combatSystem.Hit(info.damage, data.Defence);
        CurHp -= finalDamage;
        if (!isDead)
        {
            stateMachine.ChangeState(StateType.Hit);
        }
        GlobalEvents.PrintDamage(finalDamage.ToString(), transform);
    }
    public virtual void Die()
    {
        if (isDead) return;
        isDead = true;
        stateMachine.ChangeState(StateType.Die);
    }
    public void Refresh()
    {
        CurHp = data.MaxHp;
        isDead = false;

        foreach (var skilldata in skills)
        {
            skilldata.skill.isCooltime = false;
            skilldata.skill.currentCooltime = 0;
        }
        basicAttack.isCooltime = false;
        basicAttack.currentCooltime = 0;

        PoolManager.instance.PoolDic[PoolType.Monster].ReturnPool(this);
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
        return detection.Target.transform.position;
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

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
