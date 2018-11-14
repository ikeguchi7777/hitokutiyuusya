using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
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

public abstract class PlayerUserControl<T> : StatefulObjectBase<T, PlayerState>, IDamageable,IPlayerEvent
    where T : PlayerUserControl<T>
{
    protected int id;
    bool isDamage = false;
    protected PlayerMover playerMover;
    public GameObject LockOnObject { get; private set; }
    private Subject<float> HPSubject = new Subject<float>();
    private Subject<float> strongSubject = new Subject<float>();
    private Subject<float> specialSubject = new Subject<float>();

    [SerializeField] float hp = 100, Defence = 3, WincePoint = 1;
    [SerializeField] float strongrecasttime, specialrecasttime;
    Recast strongRecast, specialRecast;

    float Health
    {
        get { return hp; }
        set
        {
            hp = value;
            if (hp < 0.0f)
                hp = 0.0f;
            HPSubject.OnNext(hp);
        }
    }

    public Subject<float> RemainHP
    {
        get
        {
            return HPSubject;
        }
    }

    public Subject<float> SpecialGage
    {
        get
        {
            return specialSubject;
        }
    }

    public Subject<float> StrongGage
    {
        get
        {
            return strongSubject;
        }
    }

    protected override void Update()
    {
        base.Update();
        specialRecast.AddTime(Time.deltaTime);
        specialSubject.OnNext(specialRecast.rate());
        strongRecast.AddTime(Time.deltaTime);
        strongSubject.OnNext(strongRecast.rate());
        if (PlayerInput.PlayerInputs[id].GetButtonDown(EButton.LockOn))
        {
            playerMover.SwitchLockOn();
        }
        var camh = PlayerInput.PlayerInputs[id].GetAxis(EAxis.CamX);
        var camv = PlayerInput.PlayerInputs[id].GetAxis(EAxis.CamY);
        playerMover.CameraMove(camh, camv);
    }

    private void Start()
    {
        GetComponent<pointcache.Minimap.MinimapObject>().SetIcon(id);
    }

    protected override void Awake()
    {
        base.Awake();
        playerMover = GetComponent<PlayerMover>();
        strongRecast = new Recast(strongrecasttime);
        specialRecast = new Recast(specialrecasttime);
    }

    public void setParams(int id)
    {
        this.id = id;
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
        if (ScoreBoard.Instance.isWin)
            return;
        ScoreBoard.Instance.isNoDamage[id] = false;
        var damage = Mathf.Clamp((Random.value <= cri ? 0 : -Defence) + atk, 0, float.MaxValue);
        Debug.Log(damage);
        Health -= damage;
        if (Health <= 0)
            ChangeState(PlayerState.Death);
        else if (damage > WincePoint)
            StartCoroutine(Damage());
        ScoreBoard.Instance.RemainHP[id] = Health;
    }

    protected void Attack(PlayerState type)
    {
        playerMover.Attack((int)type - (int)PlayerState.WeakAttack);
    }

    public void HealHP(float value)
    {
        StartCoroutine(CHealHP(value));
    }

    IEnumerator CHealHP(float value)
    {
        float time = 0.0f;
        while (time<=3.0f)
        {
            time += Time.deltaTime;
            Health += value * Time.deltaTime;
            yield return null;
        }
    }

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
                return;
            }
            else if (owner.strongRecast.Useable && PlayerInput.PlayerInputs[owner.id].GetButtonDown(EButton.StrongAttack))
            {
                owner.strongRecast.Useable = false;
                owner.ChangeState(PlayerState.StrongAttack);
                return;
            }
            else if (owner.specialRecast.Useable && PlayerInput.PlayerInputs[owner.id].GetButtonDown(EButton.SpecialAttack))
            {
                owner.specialRecast.Useable = false;
                owner.ChangeState(PlayerState.SpecialAttack);
                return;
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

        public float rate()
        {
            return time / recast;
        }
    }
}