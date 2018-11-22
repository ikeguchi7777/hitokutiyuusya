using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BossCollider : MonoBehaviour {
    [SerializeField] float attack, critical;
    [SerializeField] SEType se = SEType.None;

    private void OnTriggerEnter(Collider other)
    {
        SEController.Instance.PlaySE(se);
        ExecuteEvents.Execute<IDamageable>(other.gameObject, null, (reciever, eventData) => reciever.Damage(attack, critical));
    }

    private void OnParticleCollision(GameObject other)
    {
        ExecuteEvents.Execute<IDamageable>(other, null, (reciever, eventData) => reciever.Damage(attack, 1));
    }
}
