using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public abstract class EnemyControl<T, TEnum> : StatefulObjectBase<T, TEnum>, LockOnable, IDamageable
where T : EnemyControl<T, TEnum> where TEnum : System.IConvertible
{
    private static int enemyNum = 0;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected EnemyWeapon enemyWeapon;
    protected float defaultAngularSpeed, defaultSpeed;
    protected float stopDistance = 1.5f;
    private Transform _target;
    private bool alive = true;

    [SerializeField] float Health, Attack, Defence, WincePoint;
    [SerializeField, Range(0, 1)] float Critical;
    [SerializeField] protected float MaxWalkSpeed = 2.451658f;

    public int ID { get; set; }

    public int cameraFlag { get; set; }
    public bool AttackFlag { protected get; set; }
    protected Transform target
    {
        get
        {
            if (InstantiateObjectManager.Instance.PlayerList.FindIndex(player => player.transform == _target) == -1)
                SetTarget();
            return _target;
        }
        set
        {
            _target = value;
        }
    }

    public Transform TargetTransform
    {
        get
        {
            return transform;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        ID = enemyNum;
        enemyNum++;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyWeapon = GetComponentInChildren<EnemyWeapon>();
        defaultAngularSpeed = agent.angularSpeed;
        defaultSpeed = agent.speed;
        AttackFlag = false;
        if (InstantiateObjectManager.Instance)
            InstantiateObjectManager.Instance.EnemyList.Add(this);
    }

    private void LateUpdate()
    {
        cameraFlag = 0;
    }

    void AttackStart()
    {
        enemyWeapon.Activate(Attack, Critical);
    }

    void AttackEnd()
    {
        enemyWeapon.DeActive();
    }

    protected virtual void OnDestroy()
    {
        if (InstantiateObjectManager.Instance)
            InstantiateObjectManager.Instance.RemoveEnemy(this);
    }

    void IDamageable.Damage(float atk, float cri)
    {
        if (!alive)
            return;
        var damage = Mathf.Clamp((Random.value <= cri ? 1.5f : 1) * atk - Defence, 0, float.MaxValue);
        Health -= damage;
        if (Health <= 0)
        {
            Death();
            alive = false;
        }
        else if (damage > WincePoint)
            Wince();
    }

    protected virtual void Death()
    {
        Destroy(gameObject);
    }

    //怯み
    abstract protected void Wince();

    protected bool SetTarget()
    {
        var count = InstantiateObjectManager.Instance.PlayerList.Count;
        if (count == 0)
            return false;
        _target = InstantiateObjectManager.Instance.PlayerList[Random.Range(0, count)].transform;
        return true;
    }
}

public interface LockOnable
{
    int ID { get; set; }
    int cameraFlag { get; set; }
    Transform TargetTransform { get; }
}