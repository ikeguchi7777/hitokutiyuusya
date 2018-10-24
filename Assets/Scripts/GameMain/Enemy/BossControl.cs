using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    Wait,
    Idle
}
public class BossControl : EnemyControl<BossControl, BossState>
{
    protected override BossState GetFirstState()
    {
        return BossState.Wait;
    }

    protected override void StateListInit()
    {
        stateList.Add(new StateIdle(this));
    }

    protected override void Wince()
    {
        
    }

    class StateWait : State<BossControl>
    {
        public StateWait(BossControl owner) : base(owner, BossState.Wait)
        {
        }
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
