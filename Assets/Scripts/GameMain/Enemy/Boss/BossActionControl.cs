using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActionControl : MonoBehaviour
{
    [SerializeField] ParticleSystem breath;
    [SerializeField] Collider r_hand, l_hand, hip;
    BossAttackSE AttackSE;

    private void Awake()
    {
        AttackSE = GetComponent<BossAttackSE>();
    }

    public void SetBreath(int isActive)
    {
        if (isActive == 1)
        {
            breath.Play(true);
            AttackSE.BreathSE(true);
        }
        else
        {
            breath.Stop(true);
            AttackSE.BreathSE(false);
        }
    }

    public void Shout(int isActive)
    {
        AttackSE.ShoutSE(isActive == 1);
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

    public void StopAll()
    {
        SetBreath(0);
        SetLHand(0);
        SetRHand(0);
        SetHip(0);
        Shout(0);
    }
}
