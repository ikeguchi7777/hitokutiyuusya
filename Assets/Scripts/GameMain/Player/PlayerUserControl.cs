﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Wait,
    Moveable,
    Evade,
    WeakAttack,
    StrongAttack,
    SpecialAttack
}

public abstract class PlayerUserControl<T> : StatefulObjectBase<T, PlayerState>, IDamageable
    where T : PlayerUserControl<T>
{
    static int _id = 0;
    int id = 0;
    private PlayerMover playerMover;
    public GameObject LockOnObject { get; private set; }
    [SerializeField] float strongrecasttime, specialrecasttime;
    Recast strongRecast, specialRecast;

    protected override void Update()
    {
        base.Update();
        specialRecast.AddTime(Time.deltaTime);
        strongRecast.AddTime(Time.deltaTime);
        if (PlayerInput.PlayerInputs[id].GetButtonDown(EButton.LockOn))
        {
            playerMover.SwitchLockOn();
        }
        var camh = PlayerInput.PlayerInputs[id].GetAxis(EAxis.CamX);
        var camv = PlayerInput.PlayerInputs[id].GetAxis(EAxis.CamY);
        playerMover.CameraMove(camh, camv);
    }

    protected override void Awake()
    {
        base.Awake();
        playerMover = GetComponent<PlayerMover>();
        strongRecast = new Recast(strongrecasttime);
        specialRecast = new Recast(specialrecasttime);
        setParams();
    }

    public void setParams()
    {
        id = _id;
        _id++;
        playerMover.CameraFlag = 1 << id;
    }
    protected override PlayerState GetFirstState()
    {
        return PlayerState.Wait;
    }

    protected override void StateListInit()
    {
        stateList.Add(new StateWait(this as T));
        stateList.Add(new StateMoveable(this as T));
        stateList.Add(new StateEvade(this as T));
    }

    public void Damage(float atk, float cri)
    {

    }

    protected void Attack(PlayerState type)
    {
        playerMover.Attack((int)type - (int)PlayerState.WeakAttack);
    }

    protected abstract void AttackStart();
    protected abstract void AttackEnd();

    #region State
    private class StateWait : State<T>
    {
        public StateWait(T owner) : base(owner, PlayerState.Wait)
        {
        }
    }

    protected class StateMoveable : State<T>
    {
        public StateMoveable(T owner) : base(owner, PlayerState.Moveable) { }

        public override void Execute()
        {
            if (PlayerInput.PlayerInputs[id].GetButtonDown(EButton.Evade))
            {
                owner.ChangeState(PlayerState.Evade);
                return;
            }
            else if (PlayerInput.PlayerInputs[id].GetButtonDown(EButton.WeakAttackAndSubmit))
            {
                owner.ChangeState(PlayerState.WeakAttack);
            }
            else if (owner.strongRecast.Useable && PlayerInput.PlayerInputs[id].GetButtonDown(EButton.StrongAttack))
            {
                owner.strongRecast.Useable = false;
                owner.ChangeState(PlayerState.StrongAttack);
            }
            else if (owner.specialRecast.Useable && PlayerInput.PlayerInputs[id].GetButtonDown(EButton.SpecialAttack))
            {
                owner.specialRecast.Useable = false;
                owner.ChangeState(PlayerState.SpecialAttack);
            }
        }

        public override void FixedExecute()
        {
            var h = PlayerInput.PlayerInputs[id].GetAxis(EAxis.X);
            var v = PlayerInput.PlayerInputs[id].GetAxis(EAxis.Y);
            owner.playerMover.Move(h, v);
        }
    }

    protected class StateEvade : State<T>
    {
        public StateEvade(T owner) : base(owner, PlayerState.Evade) { }
        float time;

        public override void Enter()
        {
            time = 0.0f;
            owner.playerMover.Evade();
        }

        public override void Execute()
        {/*
            if (time < 0.2f)
            {
                time += Time.deltaTime;
                var x = Mathf.Sin(time * 5 * Mathf.PI) * Mathf.Sin(time * 5 * Mathf.PI) * 50.0f;
                owner.playerMover.Evade(x);
            }
            else
                owner.ChangeState(PlayerState.Moveable);*/
        }
    }
    #endregion

    class Recast
    {
        float time,recast;
        bool useable;
        public bool Useable
        {
            get
            {
                return useable;
            }
            set
            {
                useable = value;
                time = 0.0f;
            }
        }
        public Recast(float recastTime)
        {
            recast = recastTime;
        }
        
        public void AddTime(float value)
        {
            if (useable)
                return;
            time += value;
            if (time > recast)
                useable = true;
        }
    }
}