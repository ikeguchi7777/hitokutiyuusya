﻿using System.Collections;
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
    [SerializeField] float Health, Attack, Defence;
    [SerializeField, Range(0, 1)] float Critical;

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
        enemyWeapon = GetComponentInChildren<EnemyWeapon>();
        defaultAngularSpeed = agent.angularSpeed;
        defaultSpeed = agent.speed;
        AttackFlag = false;
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

    private void OnDestroy()
    {
        InstantiateObjectManager.Instance.RemoveEnemy(this);
    }

    void IDamageable.Damage(float atk, float cri)
    {
        Health -= Mathf.Clamp((Random.value <= cri ? 1.5f : 1) * atk - Defence, 0, float.MaxValue);
        if (Health <= 0)
            Death();
    }

    protected virtual void Death()
    {
        Destroy(gameObject);
    }
}

public interface LockOnable
{
    int ID { get; set; }
    int cameraFlag { get; set; }
    Transform TargetTransform { get; }
}