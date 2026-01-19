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

public class MonsterDeadState : MonsterState
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