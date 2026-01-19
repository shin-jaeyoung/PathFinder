using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    Idle,
    Move,
    Attack,
    Hit,
    Die,

}
public abstract class State<T> 
{
    public T owner;
    public StateMachine<T> stateMachine;

    public void Setup(T owner, StateMachine<T> stateMachine)
    {
        this.owner = owner;
        this.stateMachine = stateMachine;
    }

    public abstract void Enter();
    public abstract void Update();
    public virtual void FixedUpdate() { } // 물리 이동용
    public abstract void Exit();
}

public class StateMachine<T>
{
    private T owner;
    private State<T> curState;
    public State<T> CurState => curState;
    public Dictionary<StateType, State<T>> stateDic;
    public StateMachine(T owner)
    {
        this.owner = owner;
        stateDic = new Dictionary<StateType, State<T>>();
    }
    public void AddState(StateType type, State<T> state)
    {
        if (stateDic.ContainsKey(type)) return;
        state.Setup(owner, this);
        stateDic.Add(type, state);
    }
    public void ChangeState(StateType type)
    {
        if(!stateDic.ContainsKey(type)) return;
        if (curState == stateDic[type]) return;

        curState?.Exit();
        curState = stateDic[type];
        curState.Enter();
    }

    public void Update() => curState?.Update();
    public void FixedUpdate() => curState?.FixedUpdate();
}
