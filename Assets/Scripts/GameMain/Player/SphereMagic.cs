using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMagic : MonoBehaviour
{
    ParticleSystem particle;
    [SerializeField] float speed;
    [SerializeField] bool destroy;
    [SerializeField] float time = 5.0f;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        Destroy(gameObject, time);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (particle)
        {
            particle.TriggerSubEmitter(0);
            particle.TriggerSubEmitter(1);
            particle.TriggerSubEmitter(2);
            particle.Stop(false);
        }
        if (destroy)
            Destroy(gameObject);
        else
            Destroy(gameObject, 1.0f);
    }
}
