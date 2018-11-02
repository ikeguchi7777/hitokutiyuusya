using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMagic : MonoBehaviour
{
    ParticleSystem particle;
    [SerializeField] float speed;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        transform.Translate(transform.forward * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        particle.TriggerSubEmitter(0);
        particle.TriggerSubEmitter(1);
        particle.TriggerSubEmitter(2);
        particle.Stop(false);
        Destroy(gameObject, 1.0f);
    }
}
