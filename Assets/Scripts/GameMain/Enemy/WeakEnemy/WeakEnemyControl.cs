using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

public enum WeakEnemyState
{
    Wait,
    Idle,
    Chase,
    Attack,
    Damage,
    Death
}

public class WeakEnemyControl : EnemyControl<WeakEnemyControl, WeakEnemyState>
{
    Transform targetTransform;

    public void Pawn()
    {
        animator.SetBool("Pawned", true);
    }

    protected override WeakEnemyState GetFirstState()
    {
        return WeakEnemyState.Wait;
    }

    protected override void StateListInit()
    {
        stateList.Add(new StateWait(this));
        stateList.Add(new StateIdle(this));
        stateList.Add(new StateChase(this));
        stateList.Add(new StateAttack(this));
        stateList.Add(new StateDamage(this));
        stateList.Add(new StateDeath(this));
    }

    protected override void Wince()
    {
        ChangeState(WeakEnemyState.Damage);
    }

    protected override void Death()
    {
        ChangeState(WeakEnemyState.Death);
    }

    class StateWait : State<WeakEnemyControl>
    {
        public StateWait(WeakEnemyControl owner) : base(owner, WeakEnemyState.Wait)
        {
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
            var size = InstantiateObjectManager.Instance.PlayerList.Count;
            if (size == 0)
                return;
            var obj = InstantiateObjectManager.Instance.PlayerList[UnityEngine.Random.Range(0, size - 1)];
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
        { }
        public override void Enter()
        {
            owner.enemyWeapon.DeActive();
            owner.animator.SetTrigger("Damage");
            owner.agent.speed = 0.0f;
            owner.agent.angularSpeed = 0;
            owner.animator.SetFloat("Forward", 0);
            Observable.Timer(TimeSpan.FromSeconds(0.833f)).Subscribe(_ => owner.ChangeState(WeakEnemyState.Idle));
        }
    }

    class StateDeath : State<WeakEnemyControl>
    {
        public StateDeath(WeakEnemyControl owner) : base(owner, WeakEnemyState.Death)
        {
        }

        public override void Enter()
        {
            owner.animator.SetTrigger("Death");
            owner.gameObject.tag = "Untagged";
            InstantiateObjectManager.Instance.RemoveEnemy(owner);
            foreach (var item in owner.GetComponents<Collider>())
            {
                item.enabled = false;
            }
            Observable.Timer(TimeSpan.FromSeconds(2.167f)).Subscribe(_ => FadeOut());
        }

        void FadeOut()
        {
            foreach (var renderer in owner.GetComponentsInChildren<Renderer>())
            {
                foreach (var mat in renderer.materials)
                {
                    mat.DOColor(new Color(1, 1, 1, 0), 1.0f);
                }
            }
            Observable.Timer(TimeSpan.FromSeconds(1.1f)).Subscribe(_ => GameObject.Destroy(owner.gameObject));
        }
    }
}