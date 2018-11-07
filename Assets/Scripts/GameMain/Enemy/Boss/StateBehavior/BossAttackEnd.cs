﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackEnd : StateMachineBehaviour {

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("ChangeState", BossState.Idle);
    }
}
