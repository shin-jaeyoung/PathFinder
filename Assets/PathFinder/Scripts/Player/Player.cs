using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerLevelSystem levelSystem;
    [SerializeField]
    private PlayerStatusSystem statusSystem;
    [SerializeField]
    private Animator animator;

    private StateMachine<Player> stateMachine;



    public float Speed => StatusSystem.Stat[PlayerStatType.SPEED];
    public Vector2 InputVec { get; set; }
    public Rigidbody2D Rb { get; private set; }
    public Animator Animator => animator;
    public PlayerLevelSystem LevelSystem => levelSystem;
    public PlayerStatusSystem StatusSystem => statusSystem;
    public StateMachine<Player> StateMachine => stateMachine;

    public PlayerIdleState IdleState = new PlayerIdleState();
    public PlayerMoveState MoveState = new PlayerMoveState();

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        stateMachine = new StateMachine<Player>(this);
        IdleState.Setup(this, StateMachine);
        MoveState.Setup(this, StateMachine);

        StateMachine.ChangeState(IdleState);
    }

    private void Update() => stateMachine.Update();
    private void FixedUpdate() => stateMachine.FixedUpdate();

}
