using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordControl : MonoBehaviour {
    [SerializeField]Collider w_collider;
    [SerializeField] MeleeWeaponTrail trail;

    public void ActiveSword()
    {
        w_collider.enabled = true;
        trail.Emit = true;
    }

    public void DeactiveSword()
    {
        w_collider.enabled = false;
        trail.Emit = false;
    }

    public void ResetFlags()
    {
        w_collider.enabled = false;
        trail.Emit = false;
    }
}
