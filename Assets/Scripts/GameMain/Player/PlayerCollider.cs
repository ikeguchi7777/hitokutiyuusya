﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCollider : MonoBehaviour {
    [SerializeField] float attack, critical;
    [SerializeField] SEType se = SEType.None;
    private void OnTriggerEnter(Collider other)
    {
        SEController.Instance.PlaySE(se);
        ExecuteEvents.Execute<IDamageable>(other.gameObject, null, (reciever, eventData) => reciever.Damage(attack, critical));
    }

    private void OnParticleCollision(GameObject other)
    {
        SEController.Instance.PlaySE(se);
        ExecuteEvents.Execute<IDamageable>(other, null, (reciever, eventData) => reciever.Damage(attack, 1));
    }

    public void SetParam(float atk,float cri)
    {
        attack = atk;
        critical = cri;
    }
}
