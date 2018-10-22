using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionState : StateMachineBehaviour {

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("ChangeState", PlayerState.Moveable);
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("ResetFlags");
    }
}
