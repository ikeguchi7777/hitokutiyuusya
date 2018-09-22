using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchUserControl : PlayerUserControl<WitchUserControl>
{

    class WeakAttackState : State<WitchUserControl>
    {
        public WeakAttackState(WitchUserControl owner) : base(owner, PlayerState.WeakAttack) { }
    }
}
