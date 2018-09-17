using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public abstract class EnemyControl<T, TEnum> : StatefulObjectBase<T, TEnum>, LockOnable
where T : EnemyControl<T, TEnum> where TEnum : System.IConvertible
{
    private static int enemyNum = 0;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected float defaultAngularSpeed, defaultSpeed;

    public int ID { get; set; }

    public int cameraFlag { get; set; }
    public bool AttackFlag { protected get; set; }

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
        defaultAngularSpeed = agent.angularSpeed;
        defaultSpeed = agent.speed;
        AttackFlag = false;
        InstantiateObjectManager.Instance.EnemyList.Add(this);
    }

    private void LateUpdate()
    {
        cameraFlag = 0;
    }

    private void OnDestroy()
    {
        InstantiateObjectManager.Instance.RemoveEnemy(this);
    }
}

public interface LockOnable
{
    int ID { get; set; }
    int cameraFlag { get; set; }
    Transform TargetTransform { get; }
}