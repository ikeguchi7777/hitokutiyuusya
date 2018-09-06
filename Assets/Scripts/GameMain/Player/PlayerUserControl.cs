using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Moveable,
    Evade,
    WeakAttack,
    StrongAttack,
    SpecialAttack
}

public class PlayerUserControl<T> : StatefulObjectBase<T, PlayerState>
    where T : PlayerUserControl<T>
{
    PlayerInputKey PlayerKey = new PlayerInputKey(1); //debug
    private PlayerMover playerMover;
    public GameObject LockOnObject { get; private set; }

    protected override void Update()
    {
        base.Update();
        if (Input.GetButtonDown(PlayerKey.LockOn))
        {
            playerMover.SwitchLockOn();
        }
        var camh = Input.GetAxis(PlayerKey.CamX);
        var camv = Input.GetAxis(PlayerKey.CamY);
        playerMover.CameraMove(camh, camv);
    }

    protected override void Awake()
    {
        base.Awake();
        playerMover = GetComponent<PlayerMover>();
    }

    public void setParams(int id)
    {
        PlayerKey = new PlayerInputKey(id);
        playerMover.CameraFlag = 1 << (id - 1);
    }
    protected override PlayerState GetFirstState()
    {
        return PlayerState.Moveable;
    }

    protected override void StateListInit()
    {
        stateList.Add(new StateMoveable(this as T));
        stateList.Add(new StateEvade(this as T));
    }

    #region State
    protected class StateMoveable : State<T>
    {
        public StateMoveable(T owner) : base(owner) { }

        public override void Execute()
        {
            if(Input.GetButtonDown(owner.PlayerKey.Evade))
            {
                owner.ChangeState(PlayerState.Evade);
                return;
            }else if (Input.GetButtonDown(owner.PlayerKey.WeakAttack))
            {
                owner.ChangeState(PlayerState.WeakAttack);
            }
            else if (Input.GetButtonDown(owner.PlayerKey.StrongAttack))
            {
                owner.ChangeState(PlayerState.StrongAttack);
            }
            else if (Input.GetButtonDown(owner.PlayerKey.SpecialAttack))
            {
                owner.ChangeState(PlayerState.SpecialAttack);
            }
        }

        public override void FixedExecute()
        {
            var h = Input.GetAxis(owner.PlayerKey.X);
            var v = Input.GetAxis(owner.PlayerKey.Y);
            
            owner.playerMover.Move(h,v);
        }
    }

    protected class StateEvade : State<T>
    {
        public StateEvade(T owner) : base(owner) { }
        float time;

        public override void Enter()
        {
            time = 0.0f;
        }

        public override void Execute()
        {
            if (time < 0.2f)
            {
                time += Time.deltaTime;
                var x = Mathf.Sin(time*5 * Mathf.PI)* Mathf.Sin(time * 5 * Mathf.PI) * 50.0f;
                owner.playerMover.Evade(x);
            }
            else
                owner.ChangeState(PlayerState.Moveable);
        }
    }
    #endregion
    class PlayerInputKey
    {
        public PlayerInputKey(int id)
        {
            X = "GamePad" + id + "_X";
            Y = "GamePad" + id + "_Y";
            CamX = "GamePad" + id + "_CamX";
            CamY = "GamePad" + id + "_CamY";
            Evade = "GamePad" + id + "_Evade and Cancel";
            WeakAttack = "GamePad" + id + "_WeakAttack and Submit";
            StrongAttack = "GamePad" + id + "_StrongAttack";
            SpecialAttack = "GamePad" + id + "_SpecialAttack";
            Select = "GamePad" + id + "_Select";
            LockOn = "GamePad" + id + "_LockOn";
        }

        public string X { get; private set; }
        public string Y { get; private set; }
        public string CamX { get; private set; }
        public string CamY { get; private set; }
        public string Evade { get; private set; }
        public string WeakAttack { get; private set; }
        public string StrongAttack { get; private set; }
        public string SpecialAttack { get; private set; }
        public string Select { get; private set; }
        public string LockOn { get; private set; }
    }
}