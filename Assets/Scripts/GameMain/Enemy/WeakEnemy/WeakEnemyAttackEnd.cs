using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakEnemyAttackEnd : StateMachineBehaviour
{

    WeakEnemyControl enemyControl;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (enemyControl == null)
            enemyControl = animator.gameObject.GetComponent<WeakEnemyControl>();
        enemyControl.AttackFlag = true;
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (enemyControl == null)
            enemyControl = animator.gameObject.GetComponent<WeakEnemyControl>();
        enemyControl.AttackFlag = false;
    }
}
