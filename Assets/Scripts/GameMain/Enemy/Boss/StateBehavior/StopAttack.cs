using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAttack : StateMachineBehaviour
{
    BossActionControl boss;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss == null)
            boss = animator.gameObject.GetComponent<BossActionControl>();
        boss.StopAll();
    }
}
