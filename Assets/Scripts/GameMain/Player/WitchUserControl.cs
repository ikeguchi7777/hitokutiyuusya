using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchUserControl : PlayerUserControl<WitchUserControl>
{
    protected override void AttackEnd()
    {
        throw new System.NotImplementedException();
    }

    protected override void AttackStart()
    {
        throw new System.NotImplementedException();
    }

    class WeakAttackState : State<WitchUserControl>
    {
        public WeakAttackState(WitchUserControl owner) : base(owner, PlayerState.WeakAttack) { }
    }
}
