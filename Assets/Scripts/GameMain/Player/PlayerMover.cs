using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(IKLookAt))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] float m_MoveSpeed;
    [SerializeField] float m_TurnRate;
    [SerializeField] float m_TurnPow;
    [SerializeField] private bool isLockon = false;
    [SerializeField] float LockOnChangeInterval;
    private PlayerCameraControl playerCamera;

    private Animator m_Animator;
    private IKLookAt ikLookAt;
    private CharacterController characterController;
    private float speed;
    private float pastTime = 0.0f;
    private Vector3 camForward;
    private Vector3 direction;
    private Transform lockonTransform;
    public Transform LockOnObject
    {
        private set
        {
            lockonTransform = value;
            if (value)
            {
                playerCamera.LockOnTransform = lockonTransform;
                ikLookAt.lookAtObj = lockonTransform;
            }
        }
        get
        {
            return lockonTransform;
        }
    }
    public int CameraFlag { set; private get; }



    public void SwitchLockOn()
    {
        isLockon = !isLockon;
        m_Animator.SetBool("LockOn", isLockon);
        ikLookAt.enabled = isLockon;
        if (isLockon)
        {
            LockOn();
            playerCamera.ChangeState(CameraState.LockOn);
        }
        else
        {
            LockOnObject = null;
            playerCamera.ChangeState(CameraState.Default);
        }
    }

    public void LockOn()
    {
        var enemylist = InstantiateObjectManager.Instance.EnemyList.FindAll(x => (x.cameraFlag & CameraFlag) != 0);
        if (enemylist.Count == 0)
        {
            SwitchLockOn();
            return;
        }
        SetLockOnObject(enemylist);
    }
    void SetLockOnObject(List<LockOnable> enemylist)
    {
        LockOnable obj = null;
        var min = float.MaxValue;
        foreach (var enemy in enemylist)
        {
            var point = GetAngle(camForward, enemy.TargetTransform.position - transform.position);
            point += (enemy.TargetTransform.position - transform.position).magnitude;
            if (point < min)
            {
                min = point;
                obj = enemy;
            }
        }
        LockOnObject = obj.TargetTransform;
    }

    public void ChangeLockOnObject(bool right)
    {
        var enemylist = InstantiateObjectManager.Instance.EnemyList.FindAll(x => (x.cameraFlag & CameraFlag) != 0 && Vector3.Cross(LockOnObject.position - transform.position, x.TargetTransform.position - transform.position).y * (right ? 1 : -1) > 0);
        if (enemylist.Count == 0)
        {
            return;
        }
        SetLockOnObject(enemylist);
    }

    private float GetAngle(Vector3 from, Vector3 to)
    {
        return Vector3.Angle(from, to) * (Vector3.Cross(from, to).y > 0 ? 1 : -1);
    }
    public void Move(float h, float v)
    {
        camForward = Vector3.Scale(playerCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        direction = v * camForward + h * playerCamera.transform.right;
        if (direction.magnitude > 1.0f)
            direction.Normalize();
        float rad;
        var forward = direction.magnitude;
        if (isLockon)
        {
            rad = -GetAngle(camForward, transform.forward) + GetAngle(camForward, direction);

            if (v < -0.1f)
            {
                rad = (rad > 0.0f ? rad - 180.0f : rad + 180.0f);
                forward = -forward;
            }
        }
        else
            rad = GetAngle(transform.forward, direction);
        transform.localEulerAngles += new Vector3(0, Mathf.Clamp(rad, -m_TurnRate, m_TurnRate), 0);
        speed = m_MoveSpeed * Mathf.Pow((1 - Mathf.Abs(rad) / 180.0f), m_TurnPow);
        m_Animator.SetFloat("Forward", forward);
    }

    public void Evade(float x)
    {
        float rad;
        var forward = m_Animator.GetFloat("Forward");
        if (isLockon)
        {
            rad = -GetAngle(camForward, transform.forward) + GetAngle(camForward, direction);

            if (forward < 0.0f)
            {
                rad = (rad > 0.0f ? rad - 180.0f : rad + 180.0f);
                forward = -1;
            }
            else forward = 1;
        }
        else
            rad = GetAngle(transform.forward, direction);
        transform.localEulerAngles += new Vector3(0, Mathf.Clamp(rad, -m_TurnRate, m_TurnRate), 0);
        speed = 0;
        characterController.Move(direction * x * Time.deltaTime);
    }

    public void setCamera(PlayerCameraControl cam, float y)
    {
        playerCamera = cam;
        playerCamera.setParams(transform, y);
    }

    public void CameraMove(float h, float v)
    {
        playerCamera.Pitch += v * playerCamera.PitchRate;
        if (isLockon)
        {
            if (Time.time - pastTime > LockOnChangeInterval)
            {
                ChangeLockOnObject(h > 0.0f);
                pastTime = Time.time;
            }
        }
        else
            playerCamera.Yaw += h * playerCamera.YawRate;
    }
    private void OnAnimatorMove()
    {
        var v = m_Animator.deltaPosition * speed;
        characterController.Move(v);
    }

    void Awake()
    {
        m_Animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        ikLookAt = GetComponent<IKLookAt>();
    }
    private void OnDestroy()
    {
        Destroy(playerCamera.gameObject);
    }
}
