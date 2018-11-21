using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordmanControl : PlayerUserControl<SwordmanControl> {
    [SerializeField] PlayerCollider sword;
    protected override void StateListInit()
    {
        base.StateListInit();
        stateList.Add(new StateWeakAttack(this));
        stateList.Add(new StateStrongAttack(this));
        stateList.Add(new StateSpecialAttack(this));
    }

    public void SetWeaponParams(float attack,float critical)
    {
        sword.SetParam(attack, critical);
    }

    class StateWeakAttack : State<SwordmanControl>
    {
        public StateWeakAttack(SwordmanControl owner) : base(owner, PlayerState.WeakAttack) { }

        public override void Enter()
        {
            owner.Attack(PlayerState.WeakAttack);
        }

        public override void Execute()
        {
            if (PlayerInput.PlayerInputs[owner.id].GetButtonDown(EButton.WeakAttackAndSubmit))
                owner.Attack(PlayerState.WeakAttack);
        }
    }
    class StateStrongAttack : State<SwordmanControl>
    {
        public StateStrongAttack(SwordmanControl owner) : base(owner, PlayerState.StrongAttack) { }

        public override void Enter()
        {
            owner.Attack(PlayerState.StrongAttack);
        }
    }
    class StateSpecialAttack : State<SwordmanControl>
    {
        public StateSpecialAttack(SwordmanControl owner) : base(owner, PlayerState.SpecialAttack) { }

        public override void Enter()
        {
            owner.Attack(PlayerState.SpecialAttack);
        }
    }
}
