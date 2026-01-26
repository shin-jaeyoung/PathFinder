using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonster : Monster
{
    protected override void InitStart()
    {
        //상태패턴 추가해줘야함
        stateMachine.AddState(StateType.Idle, new MonsterIdleState());
        stateMachine.AddState(StateType.Move, new MonsterMoveState());
        stateMachine.AddState(StateType.Goback, new MonsterGobackState());
        stateMachine.AddState(StateType.Attack, new MonsterAttackState());
        stateMachine.AddState(StateType.Hit, new MonsterHitState());
        stateMachine.AddState(StateType.Die, new MonsterDieState());


        stateMachine.ChangeState(StateType.Idle);
    }
}
