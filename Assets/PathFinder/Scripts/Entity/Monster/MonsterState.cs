using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterState : State<Monster> { };

public class MonsterIdleState : MonsterState 
{
    public override void Enter()
    {
        
    }
    public override void Update()
    {
        if (owner.Detection.IsDetect)
        {
            stateMachine.ChangeState(StateType.Move);
        }
    }
    public override void FixedUpdate()
    {
        
    }
    public override void Exit()
    {

    }
}

public class MonsterMoveState : MonsterState
{
    public override void Enter()
    {
        owner.Animator.SetBool("IsMove", true);
    }
    public override void Update()
    {
        if (owner.IsDead) return;
        if (!owner.Detection.IsDetect)
        {
            stateMachine.ChangeState(StateType.Goback);
            return;
        }
        if (owner.CanUseAnySkill)
        {
            stateMachine.ChangeState(StateType.Attack);
            return;
        }
        if (owner.Detection.IsInAttackRange())
        {
            stateMachine.ChangeState(StateType.Attack);
            return;
        }
    }
    public override void FixedUpdate()
    {
        if (owner.Detection.Target == null) return;

        owner.Detection.UpdateVisibility();

        Vector2 moveTargetPos;

        if (owner.Detection.IsTargetVisible)
        {
            moveTargetPos = owner.Detection.Target.position;
        }
        else
        {
            moveTargetPos = owner.Detection.LastKnownPos;

            // 마지막 위치 도착 확인
            float distToLastPos = Vector2.Distance(owner.transform.position, moveTargetPos);
            if (distToLastPos < 0.3f)
            {
                // 수색 상태로 전환
                stateMachine.ChangeState(StateType.Search);
                return;
            }
        }

        //너무 붙으면 속도 멈추게
        float dist = Vector2.Distance(owner.transform.position, owner.Detection.Target.position);
        if(dist < 0.8f)
        {
            owner.Rb.velocity = Vector2.zero;
            return;
        }
        // 플레이어 방향으로 이동
        Vector2 dir = owner.Detection.GetAdjustedDirection(moveTargetPos);
        owner.Rb.velocity = dir * owner.Data.Speed;
        owner.FlipSprite(dir.x);
    }
    public override void Exit()
    {
        owner.Rb.velocity = Vector2.zero;
        owner.Animator.SetBool("IsMove", false);
    }
}
public class MonsterSearchState : MonsterState
{


    public override void Enter()
    {

        owner.Rb.velocity = Vector2.zero;
        //Debug.Log("수색 시작");
        owner.Detection.StartTrackingWatch();
    }

    public override void Update()
    {
        owner.Detection.UpdateVisibility();
        if (owner.Detection.IsTargetVisible)
        {
            stateMachine.ChangeState(StateType.Move);
            return;
        }
        if (owner.Detection.IsDetect==false)
        {
            stateMachine.ChangeState(StateType.Goback);
            return;
        }
    }

    public override void Exit()
    {
        owner.Detection.StopTrackingWatch();
    }
}
public class MonsterGobackState : MonsterState
{
    public override void Enter()
    {
        owner.Animator.SetBool("IsMove", true);
    }
    public override void Update()
    {
        if (owner.Detection.IsDetect)
        {
            stateMachine.ChangeState(StateType.Move);
            return;
        }
        float distToOrigin = Vector2.Distance(owner.transform.position, owner.Detection.OriginVec);
        if (distToOrigin < 0.1f)
        {
            stateMachine.ChangeState(StateType.Idle);
        }

    }
    public override void FixedUpdate()
    {
        Vector2 dir = owner.Detection.GetAdjustedDirection(owner.Detection.OriginVec);
        owner.Rb.velocity = dir * owner.Data.Speed;
        owner.FlipSprite(dir.x);
    }
    public override void Exit()
    {
        owner.Rb.velocity = Vector2.zero;
        owner.Animator.SetBool("IsMove", false);
    }
}
public class MonsterHitState : MonsterState
{
    private Coroutine hitRoutine;
    public override void Enter()
    {
        //애니메이션 연출만 하게하고
        owner.Animator.SetTrigger("Hit");
        stateMachine.ChangeState(StateType.Idle);
    }
    public override void Update()
    {

    }
    public override void FixedUpdate()
    {

    }
    public override void Exit()
    {

    }
}

public class MonsterAttackState : MonsterState
{
    private bool isAttack = false;
    private Coroutine attackRoutine;
    public override void Enter()
    {
        Debug.Log("몬스터 공격");
        isAttack = true;
        //발동
        //owner.Animator.SetTrigger($"Attack{attackanimnum}";
        owner.Active(0);
        if(owner.AttackAnimNum == -1)
        {
            owner.Animator.SetTrigger("Attack");
        }
        else
        {
            owner.Animator.SetTrigger(($"Skill{owner.AttackAnimNum}").Trim());
        }
        //애니메이션 종료체크로직이후 isAttack = false; 1f대신 애니메이션 길이가 들어가야함

        attackRoutine = owner.StartCoroutine(WaitAnimCo(2f, AttackFalse));
    }

    public override void Update()
    {
        if(isAttack == false)
        {
            stateMachine.ChangeState(StateType.Move);
        }
    }
    public override void FixedUpdate()
    {

    }
    public override void Exit()
    {
        if(attackRoutine != null)
        {
            owner.StopCoroutine(attackRoutine);
            attackRoutine = null;
        }
        AttackFalse();
    }
    public void AttackFalse()
    {
        isAttack = false;
    }
}

public class MonsterDieState : MonsterState
{
    public override void Enter()
    {
        owner.Rb.velocity = Vector2.zero;
        //애니메이션 작동 + action에 초기화부터 리턴풀까지 넣어주기
        owner.Animator.SetBool("IsDead", true);
        //리턴풀? + 애니메이션 끝나면 idle상태로 만들어주기
        owner.StartCoroutine(WaitAnimCo(1.5f, ReturnPool));
    }
    public override void Update()
    {

    }
    public override void FixedUpdate()
    {
        owner.Rb.velocity = Vector2.zero;
    }
    public override void Exit()
    {
        owner.Animator.SetBool("IsDead", false);
    }
    public void ReturnPool()
    {
        RewardManager.instance.Reward(owner.RewardData, owner.transform.position);
        owner.Refresh();
        stateMachine.ChangeState(StateType.Idle);
    }
}

//Boss용
public class MonsterImmortalState : MonsterState
{
    
    public override void Enter()
    {
        Debug.Log("보스무적상태");
        owner.Animator.SetTrigger("Skill2");
    }
    public override void Update()
    {

    }
    public override void FixedUpdate()
    {
        
    }
    public override void Exit()
    {
        Debug.Log("보스무적끝");
    }
    
}
