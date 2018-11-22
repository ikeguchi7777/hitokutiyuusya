using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackSE : MonoBehaviour {
    [SerializeField] AudioClip shout;
    [SerializeField] AudioClip breath;
    AudioSource source;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    public void ShoutSE(bool isActive)
    {
        source.clip = shout;
        if (isActive)
            source.Play();
        else
            source.Stop();
    }
    public void BreathSE(bool isActive)
    {
        source.clip = breath;
        if (isActive)
            source.Play();
        else
            source.Stop();
    }
}
