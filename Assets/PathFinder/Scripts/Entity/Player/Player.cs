using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity
{
    [Header("Initial Stat")]
    [SerializeField]
    private PlayerStatList initStat;
    [Header("Level System")]
    [SerializeField]
    private PlayerLevelSystem levelSystem;
    [Header("Status System")]
    [SerializeField]
    private PlayerStatusSystem statusSystem;
    [Header("Inventory")]
    [SerializeField]
    private PlayerInventory inventory;
    [Header("Skill")]
    [SerializeField]
    private PlayerSkillInventory skills;

    [Header("Combat")]
    [SerializeField]
    private CombatSystem combatSystem;

    [SerializeField]
    private Animator animator;


    private StateMachine<Player> stateMachine;
    private HpPotion potion;
    private PlayerController playerController;

    //property
    public Vector2 InputVec { get; set; }
    public Rigidbody2D Rb { get; private set; }
    public PlayerStatList InitStat => initStat;
    public Animator Animator => animator;
    public PlayerLevelSystem LevelSystem => levelSystem;
    public PlayerStatusSystem StatusSystem => statusSystem;
    public PlayerInventory Inventory => inventory;
    public PlayerSkillInventory Skills => skills;
    public StateMachine<Player> StateMachine => stateMachine;
    public HpPotion Potion => potion;
    public PlayerController PlayerController => playerController;
    //state
    public PlayerIdleState IdleState = new PlayerIdleState();
    public PlayerMoveState MoveState = new PlayerMoveState();
    public PlayerDieState dieState = new PlayerDieState();
    public PlayerHitState hitState = new PlayerHitState();
    public PlayerAttackState AttackState = new PlayerAttackState();
    public PlayerDashState DashState = new PlayerDashState();
    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();

        statusSystem.Init(initStat);
        inventory.Init();
        skills.Init();
        combatSystem.Init(this);

        stateMachine = new StateMachine<Player>(this);
        stateMachine.AddState(StateType.Idle, IdleState);
        stateMachine.AddState(StateType.Move, MoveState);
        stateMachine.AddState(StateType.Attack, AttackState);
        stateMachine.AddState(StateType.Hit, hitState);
        stateMachine.AddState(StateType.Die, dieState);
        stateMachine.AddState(StateType.Dash, DashState);

        StateMachine.ChangeState(StateType.Idle);

        potion = GetComponent<HpPotion>();

        statusSystem.OnDead += Die;
    }

    private void Update() 
    { 
        stateMachine.Update(); 
    }
    private void FixedUpdate()
    { 
        stateMachine.FixedUpdate(); 
    }


    public void FlipSprite(float xInput)
    {
        if (xInput == 0) return;

        float yRotation = (xInput > 0) ? 180f : 0f;

        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    public void Die()
    {
        stateMachine.ChangeState(StateType.Die);
    }

    public override void Active(int index)
    {
        if(combatSystem.PerformSkill(skills.Skillequip[index]))
        {
            stateMachine.ChangeState(StateType.Attack);
        }
    }
    public bool Dash()
    {
        if (combatSystem.PerformSkill(skills.DashSkill))
        {
            stateMachine.ChangeState(StateType.Dash);
            return true;
        }
        return false;
    }
    public override void Hit(DamageInfo info)
    {
        float finalDamage = combatSystem.Hit(info.damage, statusSystem.Stat[PlayerStatType.Armor]);
        statusSystem.ReduceStat(PlayerStatType.CurHp, (int)finalDamage);
        Debug.Log(statusSystem.FinalStat[PlayerStatType.CurHp]);
        if(stateMachine.CurState != stateMachine.stateDic[StateType.Hit]||stateMachine.CurState != stateMachine.stateDic[StateType.Dash])
        {
            stateMachine.ChangeState(StateType.Hit);
        }
    }
    public override float GetAttackPower()
    {
        return statusSystem.FinalStat[PlayerStatType.Power];
    }
    public override EntityType GetEntityType()
    {
        return EntityType.Player;
    }
    public override Vector2 LookDir()
    {
        return playerController.mouseDir;
    }
    public override Vector2 SkillSpawnPos()
    {
        return playerController.mousePos;
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
