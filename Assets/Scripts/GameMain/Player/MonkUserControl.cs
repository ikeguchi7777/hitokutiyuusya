using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkUserControl : PlayerUserControl<MonkUserControl> {
    [SerializeField] GameObject socket, heal, weak, strong;
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

    public void Heal()
    {
        foreach (var player in InstantiateObjectManager.Instance.PlayerList)
        {
            Instantiate(heal, player.transform);
        }
    }

    public void StrongAttack()
    {
        Quaternion quaternion;
        if (playerMover.LockOnObject == null)
            quaternion = transform.rotation;
        else
            quaternion = Quaternion.LookRotation(playerMover.LockOnObject.position - transform.position);
        Instantiate(strong, socket.transform.position, quaternion);
    }

    public void WeakAttack()
    {
        Quaternion quaternion;
        if (playerMover.LockOnObject == null)
            quaternion = transform.rotation;
        else
            quaternion = Quaternion.LookRotation(playerMover.LockOnObject.position - transform.position);
        Instantiate(weak, socket.transform.position, quaternion);
    }
}
