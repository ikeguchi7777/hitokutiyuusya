using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkUserControl : PlayerUserControl<MonkUserControl> {
    protected override void AttackEnd()
    {
        throw new NotImplementedException();
    }

    protected override void AttackStart()
    {
        throw new NotImplementedException();
    }

    protected override void StateListInit()
    {
        base.StateListInit();
        stateList.Add(new StateWeakAttack(this));
        stateList.Add(new StateStrongAttack(this));
        stateList.Add(new StateSpecialAttack(this));
    }

    class StateWeakAttack : State<MonkUserControl>
    {
        public StateWeakAttack(MonkUserControl owner) : base(owner, PlayerState.WeakAttack) { }

        public override void Enter()
        {
            owner.Attack(PlayerState.WeakAttack);
        }
    }
    class StateStrongAttack : State<MonkUserControl>
    {
        public StateStrongAttack(MonkUserControl owner) : base(owner, PlayerState.StrongAttack) { }

        public override void Enter()
        {
            owner.Attack(PlayerState.StrongAttack);
        }
    }
    class StateSpecialAttack : State<MonkUserControl>
    {
        public StateSpecialAttack(MonkUserControl owner) : base(owner, PlayerState.SpecialAttack) { }

        public override void Enter()
        {
            owner.Attack(PlayerState.SpecialAttack);
        }
    }
}
