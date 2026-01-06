using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T> 
{
    protected T owner;
    protected StateMachine<T> stateMachine;

    public void Setup(T owner, StateMachine<T> stateMachine)
    {
        this.owner = owner;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { } // 물리 이동용
    public virtual void Exit() { }
}

public class StateMachine<T>
{
    private T owner;
    private State<T> curState;
    public State<T> CurState => curState;

    public StateMachine(T owner)
    {
        this.owner = owner;
    }

    public void ChangeState(State<T> newState)
    {
        if (newState == null || curState == newState) return;

        curState?.Exit();
        curState = newState;
        curState.Enter();
    }

    public void Update() => curState?.Update();
    public void FixedUpdate() => curState?.FixedUpdate();
}
