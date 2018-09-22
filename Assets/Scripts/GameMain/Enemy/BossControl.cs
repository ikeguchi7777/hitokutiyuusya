using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    Idle
}
public class BossControl : EnemyControl<BossControl, BossState>
{
    protected override BossState GetFirstState()
    {
        return BossState.Idle;
    }

    protected override void StateListInit()
    {
        stateList.Add(new StateIdle(this));
    }

    class StateIdle : State<BossControl>
    {
        public StateIdle(BossControl owner) : base(owner, BossState.Idle)
        {
        }
        public override void Enter()
        {
            
        }
    }
}
