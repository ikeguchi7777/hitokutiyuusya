using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyWeapon : MonoBehaviour
{
    Collider[] weaponCollider;
    float attack, critical;

    private void Awake()
    {
        weaponCollider = GetComponents<Collider>();
        DeActive();
    }

    public void DeActive()
    {
        foreach (var col in weaponCollider)
        {
            col.enabled = false;
        }
    }

    public void Activate(float atk, float cri)
    {
        foreach (var col in weaponCollider)
        {
            col.enabled = true;
        }
        attack = atk;
        critical = cri;
    }

    private void OnTriggerEnter(Collider other)
    {
        ExecuteEvents.Execute<IDamageable>(other.gameObject, null, (reciever, eventData) => reciever.Damage(attack, critical));
    }
}
