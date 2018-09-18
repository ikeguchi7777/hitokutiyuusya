using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public enum WeakEnemyState
{
    Idle,
    Chase,
    Attack,
    Damage
}

public class WeakEnemyControl : EnemyControl<WeakEnemyControl, WeakEnemyState>
{
    [SerializeField]
    bool isDamage;
    Transform targetTransform;

    protected override WeakEnemyState GetFirstState()
    {
        return WeakEnemyState.Idle;
    }

    protected override void StateListInit()
    {
        stateList.Add(new StateIdle(this));
        stateList.Add(new StateChase(this));
        stateList.Add(new StateAttack(this));
        stateList.Add(new StateDamage(this));
    }

    protected override void Update()
    {
        base.Update();
        if (isDamage == true)
        {
            isDamage = false;
            ChangeState(WeakEnemyState.Damage);
        }
    }

    class StateIdle : State<WeakEnemyControl>
    {
        public StateIdle(WeakEnemyControl owner) : base(owner, WeakEnemyState.Idle)
        { }

        public override void Execute()
        {
            if (owner.AttackFlag == true)
                return;
            var obj = GameObject.FindGameObjectWithTag("Player");
            if (obj)
            {
                owner.targetTransform = obj.transform;
                owner.ChangeState(WeakEnemyState.Chase);
            }
        }
    }

    class StateChase : State<WeakEnemyControl>
    {
        public StateChase(WeakEnemyControl owner) : base(owner, WeakEnemyState.Chase) { }

        public override void Enter()
        {
            owner.agent.speed = owner.defaultSpeed;
            owner.agent.angularSpeed = owner.defaultAngularSpeed;
        }

        public override void Execute()
        {
            if (owner.targetTransform)
            {
                owner.agent.SetDestination(owner.targetTransform.position);
                if (owner.agent.remainingDistance < owner.agent.stoppingDistance)
                {
                    owner.ChangeState(WeakEnemyState.Attack);
                }
                owner.animator.SetFloat("Forward", owner.agent.velocity.magnitude / owner.agent.speed);
            }
            else
                owner.ChangeState(WeakEnemyState.Idle);

        }
    }

    class StateAttack : State<WeakEnemyControl>
    {
        public StateAttack(WeakEnemyControl owner) : base(owner, WeakEnemyState.Attack) { }

        public override void Enter()
        {
            owner.animator.SetBool("Attack", true);
            owner.agent.speed = 0.0f;
            owner.agent.angularSpeed /= 3;
        }

        public override void Execute()
        {
            if (owner.targetTransform && owner.agent.remainingDistance < owner.agent.stoppingDistance)
                owner.agent.SetDestination(owner.targetTransform.position);
            else
            {
                owner.animator.SetBool("Attack", false);
                owner.ChangeState(WeakEnemyState.Idle);
            }
        }

        public override void Exit()
        {
            owner.animator.SetFloat("Forward", 0.0f);
        }
    }

    class StateDamage : State<WeakEnemyControl>
    {
        public StateDamage(WeakEnemyControl owner) : base(owner, WeakEnemyState.Damage)
        {}
        public override void Enter()
        {
            owner.animator.SetTrigger("Damage");
            owner.agent.speed = 0.0f;
            owner.agent.angularSpeed = 0;
            owner.animator.SetFloat("Forward", 0);
            Observable.Timer(TimeSpan.FromSeconds(0.833f)).Subscribe(_ => owner.ChangeState(WeakEnemyState.Idle));
        }
    }
}