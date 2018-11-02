using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCollider : MonoBehaviour {
    [SerializeField] float attack, critical;
    private void OnTriggerEnter(Collider other)
    {
        ExecuteEvents.Execute<IDamageable>(other.gameObject, null, (reciever, eventData) => reciever.Damage(attack, critical));
    }

    private void OnParticleCollision(GameObject other)
    {
        ExecuteEvents.Execute<IDamageable>(other, null, (reciever, eventData) => reciever.Damage(attack, 1));
    }
}
