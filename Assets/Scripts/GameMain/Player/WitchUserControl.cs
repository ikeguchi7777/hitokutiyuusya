using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchUserControl : PlayerUserControl<WitchUserControl>
{
    [SerializeField] GameObject[] WeakMagic;
    [SerializeField] GameObject StrongMagic, SpecialMagic;
    [SerializeField] Transform socket;
    protected override void AttackEnd()
    {
        throw new System.NotImplementedException();
    }

    protected override void AttackStart()
    {
        throw new System.NotImplementedException();
    }

    class StateWeakAttack : State<WitchUserControl>
    {
        public StateWeakAttack(WitchUserControl owner) : base(owner, PlayerState.WeakAttack) { }

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

    class StateStrongAttack : State<WitchUserControl>
    {
        public StateStrongAttack(WitchUserControl owner) : base(owner, PlayerState.StrongAttack) { }

        public override void Enter()
        {
            owner.Attack(PlayerState.StrongAttack);
        }
    }
    class StateSpecialAttack : State<WitchUserControl>
    {
        public StateSpecialAttack(WitchUserControl owner) : base(owner, PlayerState.SpecialAttack) { }

        public override void Enter()
        {
            owner.Attack(PlayerState.SpecialAttack);
        }
    }

    public void WeakAttack(int i)
    {
        Instantiate(WeakMagic[i - 1], socket.position, playerMover.GetAttackQuaternion());
    }

    public void StrongAttack()
    {
        Instantiate(StrongMagic, socket.position, playerMover.GetAttackQuaternion());
    }

    public void SpecialAttack()
    {
        Instantiate(SpecialMagic, socket.position, playerMover.GetAttackQuaternion());
    }

    protected override void StateListInit()
    {
        base.StateListInit();
        stateList.Add(new StateWeakAttack(this));
        stateList.Add(new StateStrongAttack(this));
        stateList.Add(new StateSpecialAttack(this));
    }
}
