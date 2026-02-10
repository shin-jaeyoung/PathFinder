using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public abstract class PlayerStateBase : State<Player> { }

// 이동 상태
public class PlayerMoveState : PlayerStateBase
{
    public override void Enter() 
    { 
        owner.Animator.SetBool("Move", true);
    }
    public override void Exit()
    {
        owner.Animator.SetBool("Move", false);
    }
    public override void Update()
    {
        if (owner.PlayerController.input.x == 0 && owner.PlayerController.input.y == 0)
        {
            owner.StateMachine.ChangeState(StateType.Idle);
        }
    }
    public override void FixedUpdate()
    {
     
        if (!owner.StatusSystem.Stat.ContainsKey(PlayerStatType.SPEED)) return;
        owner.Rb.velocity = owner.InputVec * owner.StatusSystem.Stat[PlayerStatType.SPEED];
    }
}

// 대기 상태
public class PlayerIdleState : PlayerStateBase
{
    public override void Enter()
    {
        owner.Rb.velocity = Vector2.zero;
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        if (owner.PlayerController.input.x != 0 || owner.PlayerController.input.y != 0)
        {
            owner.StateMachine.ChangeState(StateType.Move);
        }
    }
}
public class PlayerAttackState : PlayerStateBase
{
    private Coroutine attackRoutine;
    public override void Enter()
    {
        owner.Rb.velocity = Vector2.zero;
        owner.Animator.SetTrigger("Attack");
        float animLengh = owner.Animator.GetCurrentAnimatorStateInfo(0).length;
        attackRoutine = owner.StartCoroutine(WaitAnimCo(animLengh, () => {
            stateMachine.ChangeState(StateType.Idle);
        }));
    }

    public override void Exit()
    {
        if (attackRoutine != null)
        {
            owner.StopCoroutine(attackRoutine);
            attackRoutine = null;
        }
    }

    public override void Update()
    {

    }
}
public class PlayerHitState : PlayerStateBase
{
    private Coroutine hitRoutine;
    public override void Enter()
    {
        owner.IsInvincible = true;
        owner.Animator.SetTrigger("Damaged");
        float animLengh = owner.Animator.GetCurrentAnimatorStateInfo(0).length;
        hitRoutine = owner.StartCoroutine(WaitAnimCo(animLengh, () => {
            stateMachine.ChangeState(StateType.Idle);
        }));
    }

    public override void Exit()
    {
        if (hitRoutine != null)
        {
            owner.StopCoroutine(hitRoutine);
            hitRoutine = null;
        }
        owner.IsInvincible = false;
    }

    public override void Update()
    {
        
    }
}
public class PlayerDashState : PlayerStateBase
{
    private float duration = 0.3f;
    private float dashSpeed = 7f;
    private Coroutine dashRoutine;
    private Vector2 dir;
    public override void Enter()
    {
        owner.Animator.SetBool("Move", true);
        owner.IsInvincible = true;
        owner.Rb.velocity = Vector2.zero;
        dir = owner.GetMoveDir();
        if (dir.x != 0)
        {
            owner.FlipSprite(dir.x);
        }
        dashRoutine = owner.StartCoroutine(WaitAnimCo(duration, () => 
        {
            stateMachine.ChangeState(StateType.Idle);
        }));
    }

    public override void Exit()
    {
        owner.Animator.SetBool("Move", false);
        owner.Rb.velocity = Vector2.zero;
        if (dashRoutine != null)
        {
            owner.StopCoroutine(dashRoutine);
            dashRoutine = null;
        }
        owner.IsInvincible = false;
    }

    public override void Update()
    {

    }
    public override void FixedUpdate()
    {
        owner.Rb.velocity = dir * dashSpeed;
    }
}
public class PlayerDieState : PlayerStateBase
{
    private float reviveDelay = 4f;
    public override void Enter()
    {
        owner.IsInvincible = true;
        owner.Rb.velocity = Vector2.zero;
        owner.Animator.SetTrigger("Death");
        owner.Animator.SetBool("isDeath", true);
        owner.StartCoroutine(WaitAnimCo(reviveDelay, owner.Revive));
    }

    public override void Exit()
    {
        owner.Animator.SetBool("isDeath", false);
        owner.IsInvincible = false;
    }

    public override void Update()
    {

    }
}
