using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerStateBase : State<Player> { }

// 이동 상태
public class PlayerMoveState : PlayerStateBase
{
    public override void Enter() 
    { 
        Debug.Log("Move상태 진입");
        owner.Animator.SetBool("Move", true);
    }
    public override void Exit()
    {
        owner.Animator.SetBool("Move", false);
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
        Debug.Log("Idle상태 진입");
        owner.Rb.velocity = Vector2.zero;
    }

}
public class PlayerAttackState : PlayerStateBase
{
    
    public override void Enter()
    {
        
    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}
public class PlayerHitState : PlayerStateBase
{

    public override void Enter()
    {
        
    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}
public class PlayerDieState : PlayerStateBase
{

    public override void Enter()
    {
        
    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}
