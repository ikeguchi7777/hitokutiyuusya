using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwordmanAttackControl : StateMachineBehaviour
{
    [SerializeField] float attack, critical;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ExecuteEvents.Execute<SwordmanControl>(animator.gameObject, null, (reciever, eventData) => reciever.SetWeaponParams(attack, critical));
    }
}
