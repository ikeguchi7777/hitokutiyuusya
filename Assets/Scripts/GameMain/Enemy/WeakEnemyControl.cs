using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeakEnemyState
{
    Idle
}
public class WeakEnemyControl : EnemyControl<WeakEnemyControl,WeakEnemyState> {
    protected override WeakEnemyState GetFirstState()
    {
        throw new System.NotImplementedException();
    }

    protected override void StateListInit()
    {
        throw new System.NotImplementedException();
    }

    class StateIdle : State<WeakEnemyState>
    {
        public StateIdle(WeakEnemyState owner) : base(owner,WeakEnemyState.Idle)
        {

        }
    }
}
