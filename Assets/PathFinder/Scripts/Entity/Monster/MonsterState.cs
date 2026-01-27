using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterState : State<Monster> { };

public class MonsterIdleState : MonsterState 
{
    public override void Enter()
    {
        Debug.Log("monster Idle");
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
        Debug.Log("monster Move");
        owner.Detection.StartTrackingWatch();
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

        //너무 붙으면 속도 멈추게
        float dist = Vector2.Distance(owner.transform.position, owner.Detection.Target.position);
        if(dist < 0.8f)
        {
            owner.Rb.velocity = Vector2.zero;
            return;
        }
        // 플레이어 방향으로 이동
        Vector2 dir = owner.Detection.GetAdjustedDirection(owner.Detection.Target.position);
        owner.Rb.velocity = dir * owner.Data.Speed;
        owner.FlipSprite(dir.x);
    }
    public override void Exit()
    {
        owner.Detection.StopTrackingWatch();
        owner.Rb.velocity = Vector2.zero;
    }
}
public class MonsterGobackState : MonsterState
{
    public override void Enter()
    {
        Debug.Log("monster Goback");
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
        //애니메이션 종료체크로직이후 isAttack = false;
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