using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Punch_L,
        Shout,
        Combo,
        Bleath,
        RotationAttack,
        JumpAttack,
        Magic
    }

    readonly float[] interval = { 0.0f, 0.5f, 0.5f, 0.2f, 1.0f, 1.0f, 1.0f, 0.2f, 3.0f };
    BossAttackState attack = BossAttackState.None;
    protected override BossState GetFirstState()
    {
        return BossState.Wait;
    }

    protected override void StateListInit()
    {
        stateList.Add(new StateWait(this));
        stateList.Add(new StateIdle(this));
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

        }
    }

    class StateWalkToPlayer : State<BossControl>
    {
        public StateWalkToPlayer(BossControl owner) : base(owner, BossState.WalkToPlayer)
        {
        }
    }

    class StateWalkToPlayerJump : State<BossControl>
    {
        public StateWalkToPlayerJump(BossControl owner) : base(owner, BossState.WalkToPlayerJump)
        {
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
            time = 0.0f;
        }

        public override void Execute()
        {
            time += Time.deltaTime;
            if (time < owner.interval[(int)owner.attack])
                return;
            owner.animator.SetInteger("AttackType", (int)owner.attack);
            owner.animator.SetTrigger("Attack");
        }
    }

    private void OnAnimatorMove()
    {
        transform.position += animator.deltaPosition * 100;
        transform.rotation = animator.rootRotation;
    }
}
