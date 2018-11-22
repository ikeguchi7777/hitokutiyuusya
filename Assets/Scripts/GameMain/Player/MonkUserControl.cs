using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkUserControl : PlayerUserControl<MonkUserControl> {
    [SerializeField] GameObject socket, heal, weak, strong;
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
            owner.Heal();
            owner.Attack(PlayerState.SpecialAttack);
        }
    }

    public void Heal()
    {
        SEController.Instance.PlaySE(SEType.Heal);
        foreach (var player in InstantiateObjectManager.Instance.PlayerList)
        {
            Debug.Log(player);
            Instantiate(heal, player.transform);
        }
    }

    public void StrongAttack()
    {
        SEController.Instance.PlaySE(SEType.Magic);
        Instantiate(strong, socket.transform.position, playerMover.GetAttackQuaternion());
    }

    public void WeakAttack()
    {
        SEController.Instance.PlaySE(SEType.Magic);
        Instantiate(weak, socket.transform.position, playerMover.GetAttackQuaternion());
    }
}
