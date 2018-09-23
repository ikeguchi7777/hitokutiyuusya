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
    public float Pitch;
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
            if (owner.Yaw > 180)
                owner.Yaw -= 360;
            else if (owner.Yaw < -180)
                owner.Yaw += 360;
            var dir = Quaternion.Euler(owner.Pitch, owner.Yaw, 0.0f) * Vector3.back * owner.Distance;
            owner.transform.position = owner.player.position + dir;
            owner.transform.LookAt(owner.player);
        }
    }

    class StateToLockOn : State<PlayerCameraControl>
    {
        Quaternion to;
        Tweener tweener, d_tweener;
        public StateToLockOn(PlayerCameraControl owner) : base(owner, CameraState.ToLockOn) { }

        public override void Enter()
        {
            to = Quaternion.LookRotation(owner.LockOnTransform.position - owner.transform.position);
            tweener = owner.transform.DORotateQuaternion(to, 0.5f);
            d_tweener = DOTween.To(() => owner.Yaw, num => owner.Yaw = num, GetYaw(), 0.5f);
        }

        public override void Execute()
        {
            to = Quaternion.LookRotation(owner.LockOnTransform.position - owner.transform.position);
            var dir = Quaternion.Euler(owner.Pitch, owner.Yaw, 0.0f) * Vector3.back * owner.Distance;
            owner.transform.position = owner.player.position + dir;
            Debug.Log(owner.Yaw);
            if (tweener.position >= 0.5f)
                owner.ChangeState(CameraState.LockOn);
            else
            {
                var t = tweener.position + Time.deltaTime;
                tweener.ChangeEndValue(to).Goto(t);
                d_tweener.ChangeEndValue(GetYaw()).Goto(t);
            }
        }

        float GetYaw()
        {
            var dir = (owner.player.position - owner.LockOnTransform.position);
            dir = Vector3.Scale(dir, new Vector3(1, 0, 1)).normalized;
            var angle = Vector3.Angle(Vector3.back, dir) * (dir.x < 0 ? 1 : -1);
            if (angle * owner.Yaw < -180)
            {
                if (angle < 0)
                    angle += 360;
                else
                    angle -= 360;
            }
            return angle;
        }

        public override void Exit()
        {
            tweener.Kill();
            d_tweener.Kill();
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
