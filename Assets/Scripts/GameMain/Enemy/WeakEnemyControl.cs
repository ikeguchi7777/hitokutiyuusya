using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeakEnemyState
{
    Idle,
    Chase,
    Attack
}

public class WeakEnemyControl : EnemyControl<WeakEnemyControl, WeakEnemyState>
{
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
            Debug.Log("Enter");
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
            owner.defaultAngularSpeed /= 3;
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
    }
}
