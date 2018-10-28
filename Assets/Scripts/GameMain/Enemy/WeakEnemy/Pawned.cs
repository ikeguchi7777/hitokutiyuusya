using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawned : StateMachineBehaviour {

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("ChangeState", WeakEnemyState.Idle);
    }
}
