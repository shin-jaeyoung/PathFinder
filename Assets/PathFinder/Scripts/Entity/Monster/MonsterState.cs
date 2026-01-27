using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterState : State<Monster> { };

public class MonsterIdleState : MonsterState 
{
    public override void Enter()
    {
        //Debug.Log("monster Idle");
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
        //Debug.Log("monster Move");
    }
    public override void Update()
    {
        if (!owner.Detection.IsDetect)
        {
            stateMachine.ChangeState(StateType.Goback);
            return;
        }

        if (owner.Detection.IsInAttackRange())
        {
            stateMachine.ChangeState(StateType.Attack);
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
            if (distToLastPos < 0.1f)
            {
                // 수색 상태로 전환!
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
        //Debug.Log("monster Goback");
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
    }
}
public class MonsterHitState : MonsterState
{
    public override void Enter()
    {

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
    public override void Enter()
    {
        Debug.Log("몬스터 공격");
        isAttack = true;
        //발동
        owner.Active(0);
        //애니메이션 종료체크로직이후 isAttack = false; 1f대신 애니메이션 길이가 들어가야함
        owner.StartCoroutine(WaitAnimCo(1.5f, AttackFalse));
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
        //애니메이션
        //리턴풀? + 애니메이션 끝나면 idle상태로 만들어주기
    }
    public override void Update()
    {

    }
    public override void FixedUpdate()
    {
        //속도 고정 zero
    }
    public override void Exit()
    {
        //여기에 상태부터 이거저거 초기화해줘야해
    }
}