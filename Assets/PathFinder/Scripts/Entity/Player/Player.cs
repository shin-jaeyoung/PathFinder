using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public bool IsInvincible;
    private bool isDead;

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
    public bool IsDead => isDead;
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
        isDead = true;
        stateMachine.ChangeState(StateType.Die);
    }
    public void Revive()
    {
        isDead = false;
        statusSystem.Heal(statusSystem.FinalStat[PlayerStatType.MaxHp]);
        //마을씬으로 이동
        SceneManager.LoadScene(SceneType.Town.ToString());
        //마을씬 리스폰 위치로 이동
        inventory.ReduceGold((int)(inventory.Gold * 0.3f));
        GlobalEvents.Notify("사망으로 골드의 30%를 잃었습니다", 4f);
        transform.position = Vector3.zero;//임시
        stateMachine.ChangeState(StateType.Idle);
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
        if (IsInvincible) return;
        float finalDamage = combatSystem.Hit(info.damage, statusSystem.Stat[PlayerStatType.Armor]);
        statusSystem.ReduceStat(PlayerStatType.CurHp, (int)finalDamage);
        GlobalEvents.PrintDamage(finalDamage.ToString(), transform);
        if (!IsDead)
        {
            stateMachine.ChangeState(StateType.Hit);
        }
    }
    //퍼센트 처리 함수
    public void Hit(int percent)
    {
        if (IsInvincible) return;
        float finalDamage = statusSystem.FinalStat[PlayerStatType.MaxHp] * percent / 100;

        statusSystem.ReduceStat(PlayerStatType.CurHp, (int)finalDamage);
        GlobalEvents.PrintDamage(finalDamage.ToString(), transform);
        if (!IsDead)
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
    public Vector2 GetMoveDir()
    {
        if (InputVec.sqrMagnitude > 0)
        {
            return InputVec;
        }
        float facingDirX = transform.right.x;

        return new Vector2(-facingDirX, 0).normalized;
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
