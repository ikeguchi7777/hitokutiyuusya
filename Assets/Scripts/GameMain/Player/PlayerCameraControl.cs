using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum CameraState
{
    Default,
    LockOn,
    ToLockOn,
    FromLockOn
}
public class PlayerCameraControl : StatefulObjectBase<PlayerCameraControl, CameraState>
{
    [SerializeField] float Distance;

    public Transform player { set; private get; }
    public Transform LockOnTransform { set; private get; }
    [SerializeField] private float _pitch = 20.0f, pmax = 70.0f, pmin = 0.0f;
    public float Pitch {
        get { return _pitch; }
        set
        {
            _pitch = value;
            if (_pitch > pmax)
                _pitch = pmax;
            else if (_pitch < pmin)
                _pitch = pmin;
        }
    }
    public float Yaw { set; get; }
    public float PitchRate;
    public float YawRate;

    public void setParams(Transform player, float y)
    {
        this.player = player;
        Yaw = y % 360;
    }

    protected override CameraState GetFirstState()
    {
        return CameraState.Default;
    }

    protected override void StateListInit()
    {
        stateList.Add(new StateDefault(this));
        stateList.Add(new StateLockOn(this));
        stateList.Add(new StateToLockOn(this));
        stateList.Add(new StateFromLockOn(this));
    }

    class StateDefault : State<PlayerCameraControl>
    {
        public StateDefault(PlayerCameraControl owner) : base(owner, CameraState.Default) { }

        public override void Enter()
        {
            owner.Yaw = Vector3.Angle(Vector3.right, owner.transform.right) * (Vector3.Cross(Vector3.right, owner.transform.right).y > 0 ? 1 : -1);
        }

        public override void Execute()
        {
            if (owner.Yaw > 180.0f)
                owner.Yaw -= 360.0f;
            else if (owner.Yaw < -180.0f)
                owner.Yaw += 360.0f;
            var dir = Quaternion.Euler(owner.Pitch, owner.Yaw, 0.0f) * Vector3.back * owner.Distance;
            owner.transform.position = owner.player.position + dir;
            owner.transform.LookAt(owner.player);
        }
    }

    class StateToLockOn : State<PlayerCameraControl>
    {
        float time;
        float yaw;
        Quaternion quaternion;
        public StateToLockOn(PlayerCameraControl owner) : base(owner, CameraState.ToLockOn) { }

        public override void Enter()
        {
            time = 0.0f;
            owner.Yaw %= 360.0f;
            if (owner.Yaw > 180.0f)
                owner.Yaw -= 360.0f;
            else if (owner.Yaw < -180.0f)
                owner.Yaw += 360.0f;
            yaw = owner.Yaw;
            quaternion = owner.transform.rotation;
        }

        public override void FixedExecute()
        {
            if (owner.LockOnTransform == null)
                owner.ChangeState(CameraState.FromLockOn);
            owner.Yaw = Mathf.Lerp(yaw, GetYaw(), time * 2.0f);
            owner.transform.rotation = Quaternion.Lerp(quaternion, Quaternion.LookRotation(owner.LockOnTransform.position - owner.transform.position), time * 2.0f);
            var dir = Quaternion.Euler(owner.Pitch, owner.Yaw, 0.0f) * Vector3.back * owner.Distance;
            owner.transform.position = owner.player.position + dir;
            time += Time.deltaTime;
            if (time > 0.5f)
                owner.ChangeState(CameraState.LockOn);
        }

        float GetYaw()
        {
            var dir = (owner.player.position - owner.LockOnTransform.position);
            dir = Vector3.Scale(dir, new Vector3(1, 0, 1)).normalized;
            var angle = Vector3.Angle(Vector3.back, dir) * (dir.x < 0 ? 1 : -1);
            if (angle * owner.Yaw < -180.0f)
            {
                if (angle < 0)
                    angle += 360.0f;
                else
                    angle -= 360.0f;
            }
            return angle;
        }
    }

    class StateFromLockOn : State<PlayerCameraControl>
    {
        public StateFromLockOn(PlayerCameraControl owner) : base(owner, CameraState.FromLockOn) { }

        public override void Enter()
        {
            owner.ChangeState(CameraState.Default);
        }
    }

    class StateLockOn : State<PlayerCameraControl>
    {
        public StateLockOn(PlayerCameraControl owner) : base(owner, CameraState.LockOn) { }

        public override void Execute()
        {
            var dir = (owner.player.position - owner.LockOnTransform.position);
            dir = Vector3.Scale(dir, new Vector3(1, 0, 1)).normalized;
            dir = Quaternion.AngleAxis(owner.Pitch, owner.transform.right) * dir * owner.Distance;
            owner.transform.position = owner.player.position + dir;
            owner.transform.LookAt(owner.LockOnTransform);
        }
    }
}
