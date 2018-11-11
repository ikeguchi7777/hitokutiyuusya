using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffectControl : MonoBehaviour
{
    [SerializeField] float value = 10.0f;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(DestroyEffect());
        GetComponentInParent<PlayerMover>().gameObject.SendMessage("HealHP", value);
    }

    IEnumerator DestroyEffect()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }
}
