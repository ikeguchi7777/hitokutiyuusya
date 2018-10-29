using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum PlayerState
{
    Wait,
    Moveable,
    Evade,
    WeakAttack,
    StrongAttack,
    SpecialAttack,
    Death
}

public abstract class PlayerUserControl<T> : StatefulObjectBase<T, PlayerState>, IDamageable
    where T : PlayerUserControl<T>
{
    static int _id = 0;
    protected int id;
    bool isDamage = false;
    private PlayerMover playerMover;
    public GameObject LockOnObject { get; private set; }
    [SerializeField] float Health = 100, Defence = 3, WincePoint = 1;
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
        Debug.Log(id);
    }
    protected override PlayerState GetFirstState()
    {
        return PlayerState.Moveable;
    }

    protected override void StateListInit()
    {
        stateList.Add(new StateWait(this as T));
        stateList.Add(new StateMoveable(this as T));
        stateList.Add(new StateEvade(this as T));
        stateList.Add(new StateDeath(this as T));
    }

    IEnumerator Damage()
    {
        isDamage = true;
        playerMover.Damage();
        yield return new WaitForSeconds(0.5f);
        isDamage = false;
        /*float time = 0.0f;
        yield return null;
        while(time<0.5f)
        {
            time += Time.deltaTime;
            yield return null;
        }*/
    }

    public void Damage(float atk, float cri)
    {
        var damage = Mathf.Clamp((Random.value <= cri ? 0 : -Defence) + atk, 0, float.MaxValue);
        Debug.Log(damage);
        Health -= damage;
        if (Health <= 0)
            ChangeState(PlayerState.Death);
        else if (damage > WincePoint)
            StartCoroutine(Damage());
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
            if (owner.isDamage)
                return;
            if (PlayerInput.PlayerInputs[owner.id].GetButtonDown(EButton.Evade))
            {
                owner.ChangeState(PlayerState.Evade);
                return;
            }
            else if (PlayerInput.PlayerInputs[owner.id].GetButtonDown(EButton.WeakAttackAndSubmit))
            {
                owner.ChangeState(PlayerState.WeakAttack);
            }
            else if (owner.strongRecast.Useable && PlayerInput.PlayerInputs[owner.id].GetButtonDown(EButton.StrongAttack))
            {
                owner.strongRecast.Useable = false;
                owner.ChangeState(PlayerState.StrongAttack);
            }
            else if (owner.specialRecast.Useable && PlayerInput.PlayerInputs[owner.id].GetButtonDown(EButton.SpecialAttack))
            {
                owner.specialRecast.Useable = false;
                owner.ChangeState(PlayerState.SpecialAttack);
            }
        }

        public override void FixedExecute()
        {
            if (owner.isDamage)
                return;
            var h = PlayerInput.PlayerInputs[owner.id].GetAxis(EAxis.X);
            var v = PlayerInput.PlayerInputs[owner.id].GetAxis(EAxis.Y);
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

    protected class StateDeath : State<T>
    {
        public StateDeath(T owner) : base(owner, PlayerState.Death)
        {
        }

        public override void Enter()
        {
            owner.GetComponent<Rigidbody>().useGravity = false;
            owner.GetComponent<Collider>().enabled = false;
            InstantiateObjectManager.Instance.PlayerList.Remove(owner.playerMover);
            owner.playerMover.Death();
        }
    }

    #endregion

    class Recast
    {
        float time, recast;
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