using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(IKLookAt))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] float m_MoveSpeed = 1;
    [SerializeField] float m_TurnRate = 10;
    [SerializeField] float m_TurnPow = 3;
    [SerializeField] private bool isLockon = false;
    [SerializeField] float LockOnChangeInterval = 0.5f;
    private PlayerCameraControl playerCamera;

    private Animator m_Animator;
    private IKLookAt ikLookAt;
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
            if (LockOn())
                playerCamera.ChangeState(CameraState.ToLockOn);
        }
        else
        {
            LockOnObject = null;
            playerCamera.ChangeState(CameraState.FromLockOn);
        }
    }

    public bool LockOn()
    {
        var enemylist = InstantiateObjectManager.Instance.EnemyList.FindAll(x => (x.cameraFlag & CameraFlag) != 0);
        if (enemylist.Count == 0)
        {
            SwitchLockOn();
            return false;
        }
        SetLockOnObject(enemylist);
        return true;
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

    public void Evade()
    {
        m_Animator.SetTrigger("Evade");
        speed = 2.0f * m_MoveSpeed;
    }

    public void Death()
    {
        m_Animator.SetTrigger("Death");
    }

    public void Damage()
    {
        m_Animator.SetTrigger("Damage");
    }

    public void setCamera(PlayerCameraControl cam, float y)
    {
        playerCamera = cam;
        playerCamera.setParams(transform, y);
    }

    public void CameraMove(float h, float v)
    {
        playerCamera.Pitch += (v * playerCamera.PitchRate * Time.deltaTime);
        if (isLockon)
        {
            if (Time.time - pastTime > LockOnChangeInterval)
            {
                ChangeLockOnObject(h > 0.0f);
                pastTime = Time.time;
            }
        }
        else
            playerCamera.Yaw += (h * playerCamera.YawRate * Time.deltaTime);
    }
    private void OnAnimatorMove()
    {
        transform.position += m_Animator.deltaPosition * speed;
        transform.rotation = m_Animator.rootRotation;
    }

    void Awake()
    {
        m_Animator = GetComponent<Animator>();
        ikLookAt = GetComponent<IKLookAt>();
        ikLookAt.enabled = false;
    }

    public void Attack(int type)
    {
        m_Animator.SetInteger("AttackType", type);
        m_Animator.SetTrigger("Attack");
    }

    public void ResetFlags()
    {
        m_Animator.SetFloat("Forward", 0);
        m_Animator.SetInteger("AttackType", -1);
        m_Animator.ResetTrigger("Attack");
        m_Animator.ResetTrigger("Evade");
    }

    public Quaternion GetAttackQuaternion()
    {
        if (lockonTransform)
            return Quaternion.LookRotation(lockonTransform.position - transform.position);
        return transform.rotation;
    }
}
