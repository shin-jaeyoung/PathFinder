using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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

    [SerializeField]
    private Animator animator;


    private StateMachine<Player> stateMachine;



    public Vector2 InputVec { get; set; }
    public Rigidbody2D Rb { get; private set; }
    public PlayerStatList InitStat => initStat;
    public Animator Animator => animator;
    public PlayerLevelSystem LevelSystem => levelSystem;
    public PlayerStatusSystem StatusSystem => statusSystem;
    public StateMachine<Player> StateMachine => stateMachine;

    public PlayerIdleState IdleState = new PlayerIdleState();
    public PlayerMoveState MoveState = new PlayerMoveState();
    public PlayerDieState dieState = new PlayerDieState();
    public PlayerAttackState AttackState = new PlayerAttackState();
    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        statusSystem = new PlayerStatusSystem();
        statusSystem.Init(initStat);
        stateMachine = new StateMachine<Player>(this);
        IdleState.Setup(this, StateMachine);
        MoveState.Setup(this, StateMachine);
        dieState.Setup(this, StateMachine);
        AttackState.Setup(this, StateMachine);
        StateMachine.ChangeState(IdleState);

        statusSystem.OnDead += Die;
    }

    private void Update() => stateMachine.Update();
    private void FixedUpdate() => stateMachine.FixedUpdate();

    public void FlipSprite(float xInput)
    {
        if (xInput == 0) return;

        float yRotation = (xInput > 0) ? 180f : 0f;

        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    public void Die()
    {
        stateMachine.ChangeState(dieState);
    }
}
