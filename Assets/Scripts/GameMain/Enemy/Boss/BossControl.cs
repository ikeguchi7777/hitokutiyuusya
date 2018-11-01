using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum BossState
{
    Wait,
    Idle,
    WalkToPlayer,
    WalkToPlayerJump,
    Attack
}

public class BossControl : EnemyControl<BossControl, BossState>
{
    enum BossAttackState
    {
        None,
        Punch_R,
        Shout,
        Combo,
        Breath,
        RotationAttack,
        JumpAttack,
        Magic
    }

    readonly float[] interval = { 0.0f, 0.5f, 0.5f, 0.2f, 1.0f, 1.0f, 1.0f, 0.2f, 3.0f };
    [SerializeField] float breathDistance = 2.0f;

    float jumpDistance = 2.5f;
    BossAttackState attack = BossAttackState.None;
    Transform target;
    float stopDistance;

    protected override BossState GetFirstState()
    {
        return BossState.Wait;
    }

    protected override void StateListInit()
    {
        stateList.Add(new StateWait(this));
        stateList.Add(new StateIdle(this));
        stateList.Add(new StateAttack(this));
        stateList.Add(new StateWalkToPlayer(this));
        stateList.Add(new StateWalkToPlayerJump(this));
    }

    protected override void Wince()
    {

    }

    class StateWait : State<BossControl>
    {
        public StateWait(BossControl owner) : base(owner, BossState.Wait)
        {
        }
    }

    class StateIdle : State<BossControl>
    {
        public StateIdle(BossControl owner) : base(owner, BossState.Idle)
        {
        }
        public override void Enter()
        {
            owner.attack = BossAttackState.None;
        }

        public override void Execute()
        {
            if (GetNearPlayerCount() * 0.1f > Random.value)
            {
                owner.attack = BossAttackState.RotationAttack;
                owner.ChangeState(BossState.Attack);
            }
            else if (SetTarget() > owner.jumpDistance && 0.6f > Random.value)
                owner.ChangeState(BossState.WalkToPlayerJump);
            else if (0.05f> Random.value)
            {
                owner.attack = BossAttackState.Magic;
                owner.ChangeState(BossState.Attack);
            }
            else if (0.05f > Random.value)
            {
                owner.attack = BossAttackState.Shout;
                owner.ChangeState(BossState.Attack);
            }
            else
                owner.ChangeState(BossState.WalkToPlayer);
        }

        Vector2 GetDirection(Vector3 pos)
        {
            return new Vector2(owner.transform.position.x - pos.x, owner.transform.position.z - pos.z);
        }

        int GetNearPlayerCount()
        {
            int count = 0;
            foreach (var player in InstantiateObjectManager.Instance.PlayerList)
            {
                if (GetDirection(player.transform.position).magnitude <= 1.4f)
                    count++;
            }
            return count;
        }

        float SetTarget()
        {
            float min = float.MaxValue;
            foreach (var player in InstantiateObjectManager.Instance.PlayerList)
            {
                if (GetDirection(player.transform.position).magnitude < min)
                {
                    min = GetDirection(player.transform.position).magnitude;
                    owner.target = player.transform;
                }
            }
            return min;
        }
    }

    class StateWalkToPlayer : State<BossControl>
    {
        Vector3 prev;
        public StateWalkToPlayer(BossControl owner) : base(owner, BossState.WalkToPlayer)
        {
        }

        public override void Enter()
        {
            var val = Random.value;
            owner.agent.stoppingDistance = 2.0f;
            owner.agent.SetDestination(owner.target.position);
            if (val < 0.33f)
                owner.attack = BossAttackState.Punch_R;
            else if (val < 0.66f)
                owner.attack = BossAttackState.Combo;
            else
                owner.attack = BossAttackState.Breath;
            prev = owner.agent.desiredVelocity;
        }

        public override void Execute()
        {
            var t = (prev + owner.agent.desiredVelocity) / 2.0f;
            prev = owner.agent.desiredVelocity;
            owner.agent.SetDestination(owner.target.position);
            var forward = Vector3.Dot(t, owner.transform.forward);
            var angle = GetAngle(t);
            if (owner.agent.remainingDistance <= owner.agent.stoppingDistance && Mathf.Abs(angle) <= 0.1f)
            {
                owner.ChangeState(BossState.Attack);
            }
            else
            {
                owner.animator.SetFloat("Forward", forward);
                owner.animator.SetFloat("Right", angle);
            }
        }

        float GetAngle(Vector3 dir)
        {
            dir = Vector3.Scale(dir, new Vector3(1, 0, 1));
            var angle = Vector3.Angle(dir, Vector3.Scale(owner.transform.forward, new Vector3(1, 0, 1)));
            if (Vector3.Angle(dir, Vector3.Scale(owner.transform.right, new Vector3(1, 0, 1))) > 90.0f)
                angle = -angle;
            return angle / 90.0f;
        }
    }

    class StateWalkToPlayerJump : State<BossControl>
    {
        Vector3 prev;
        public StateWalkToPlayerJump(BossControl owner) : base(owner, BossState.WalkToPlayerJump)
        {
        }

        public override void Enter()
        {
            owner.agent.stoppingDistance = owner.jumpDistance;
            owner.attack = BossAttackState.JumpAttack;
            prev = owner.agent.desiredVelocity;
        }

        public override void Execute()
        {
            var t = (prev + owner.agent.desiredVelocity) / 2.0f;
            prev = owner.agent.desiredVelocity;
            owner.agent.SetDestination(owner.target.position);
            var forward = Vector3.Dot(t, owner.transform.forward);
            var angle = GetAngle(t);
            if (owner.agent.remainingDistance <= owner.agent.stoppingDistance && Mathf.Abs(angle) <= 0.1f)
            {
                owner.ChangeState(BossState.Attack);
            }
            else
            {
                owner.animator.SetFloat("Forward", forward);
                owner.animator.SetFloat("Right", angle);
            }
        }

        float GetAngle(Vector3 dir)
        {
            dir = Vector3.Scale(dir, new Vector3(1, 0, 1));
            var angle = Vector3.Angle(dir, Vector3.Scale(owner.transform.forward, new Vector3(1, 0, 1)));
            if (Vector3.Angle(dir, Vector3.Scale(owner.transform.right, new Vector3(1, 0, 1))) > 90.0f)
                angle = -angle;
            return angle / 90.0f;
        }
    }

    class StateAttack : State<BossControl>
    {
        float time;
        public StateAttack(BossControl owner) : base(owner, BossState.Attack)
        {
        }

        public override void Enter()
        {
            owner.animator.SetFloat("Forward", 0.0f);
            owner.animator.SetFloat("Right", 0.0f);
            time = 0.0f;
        }

        public override void Execute()
        {
            time += Time.deltaTime;
            if (time < owner.interval[(int)owner.attack])
                return;
            owner.animator.SetInteger("AttackType", (int)owner.attack);
            owner.animator.SetTrigger("Attack");
            owner.ChangeState(BossState.Wait);
        }
    }

    private void OnAnimatorMove()
    {
        if (IsCurrentState(BossState.Attack))
            transform.position += animator.deltaPosition * 100;
        agent.speed = animator.deltaPosition.magnitude * 100.0f / Time.deltaTime;
        transform.rotation = animator.rootRotation;
    }

    public void CallEnemy()
    {
        InstantiateObjectManager.Instance.InstantiateCallEnemy();
    }

    public void Magic()
    {

    }
}
