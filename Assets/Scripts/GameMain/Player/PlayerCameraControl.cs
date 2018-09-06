using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
    Default,
    LockOn
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
    }

    class StateDefault : State<PlayerCameraControl>
    {
        public StateDefault(PlayerCameraControl owner) : base(owner) { }

        public override void Enter()
        {
            owner.Yaw = Vector3.Angle(Vector3.right, owner.transform.right) * (Vector3.Cross(Vector3.right, owner.transform.right).y > 0 ? 1 : -1);
        }

        public override void Execute()
        {
            owner.Yaw %= 360;
            var dir = Quaternion.Euler(owner.Pitch, owner.Yaw, 0.0f) * Vector3.back * owner.Distance;
            owner.transform.position = owner.player.position + dir;
            owner.transform.LookAt(owner.player);
        }
    }
    class StateLockOn : State<PlayerCameraControl>
    {
        public StateLockOn(PlayerCameraControl owner) : base(owner) { }

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
