using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActionControl : MonoBehaviour
{
    [SerializeField] ParticleSystem breath;
    [SerializeField] Collider r_hand, l_hand, hip;

    public void SetBreath(int isActive)
    {
        if (isActive == 1)
            breath.Play(true);
        else
            breath.Stop(true);
    }

    public void SetRHand(int isActive)
    {
        r_hand.enabled = isActive == 1;
    }

    public void SetLHand(int isActive)
    {
        l_hand.enabled = isActive == 1;
    }

    public void SetHip(int isActive)
    {
        hip.enabled = isActive == 1;
    }
}
