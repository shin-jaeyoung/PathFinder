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
    }
    public override void Update()
    {
        if (!owner.Detection.IsDetect)
        {
            stateMachine.ChangeState(StateType.Goback);
            return;
        }
    }
    public override void FixedUpdate()
    {
        if (owner.Detection.Target == null) return;

        // 플레이어 방향으로 이동
        Vector2 dir = (owner.Detection.Target.position - owner.transform.position).normalized;
        owner.Rb.velocity = dir * owner.Data.Speed;
        owner.FlipSprite(dir.x);
    }
    public override void Exit()
    {
        owner.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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
        Vector2 dir = (owner.Detection.OriginVec - (Vector2)owner.transform.position).normalized;
        owner.Rb.velocity = dir * owner.Data.Speed;
        owner.FlipSprite(dir.x);
    }
    public override void Exit()
    {
        owner.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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