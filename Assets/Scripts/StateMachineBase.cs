using UnityEngine;
using System.Collections.Generic;
public class State<T>
{
    // このステートを利用するインスタンス
    protected T owner;

    public int id;

    public State(T owner, System.IConvertible id)
    {
        this.owner = owner;
        this.id = id.ToInt32(null);
    }

    /// <summary>
    /// このステートに遷移する時に一度だけ呼ばれる
    /// </summary>
    public virtual void Enter() { }

    /// <summary>
    /// このステートである間、Updateで呼ばれる
    /// </summary>
    public virtual void Execute() { }

    /// <summary>
    /// このステートである間、FixedUpdateで呼ばれる
    /// </summary>
    public virtual void FixedExecute() { }

    /// <summary>
    /// このステートから他のステートに遷移するときに一度だけ呼ばれる
    /// </summary>
    public virtual void Exit() { }
}

public class StateMachine<T>
{
    public StateMachine(State<T> firstState)
    {
        CurrentState = firstState;
        CurrentState.Enter();
    }

    public State<T> CurrentState { get; private set; }

    public void ChangeState(State<T> state)
    {
        CurrentState.Exit();
        CurrentState = state;
        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState.Execute();
    }

    public void FixedUpdate()
    {
        CurrentState.FixedExecute();
    }
}

public abstract class StatefulObjectBase<T, TEnum> : MonoBehaviour
    where T : class where TEnum : System.IConvertible
{
    protected List<State<T>> stateList = new List<State<T>>();

    protected StateMachine<T> stateMachine;

    /// <summary>
    /// 現在の状態をstateに遷移する
    /// </summary>
    public virtual void ChangeState(TEnum state)
    {
        stateMachine.ChangeState(FindState(state));
    }

    /// <summary>
    /// 現在の状態がstateならtrue
    /// </summary>
    public virtual bool IsCurrentState(TEnum state)
    {
        return stateMachine.CurrentState == FindState(state);
    }

    private State<T> FindState(TEnum state)
    {
        return stateList.Find(x => x.id == state.ToInt32(null));
    }

    /// <summary>
    /// 初期状態を返す
    /// </summary>
    protected abstract TEnum GetFirstState();

    protected virtual void Update()
    {
        stateMachine.Update();
    }

    protected virtual void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    /// <summary>
    /// stateListの中身を入れる処理
    /// </summary>
    protected abstract void StateListInit();

    /// <summary>
    /// 必ず親のawakeも呼び出す
    /// </summary>
    protected virtual void Awake()
    {
        StateListInit();
        stateMachine = new StateMachine<T>(stateList[GetFirstState().ToInt32(null)]);
    }
}