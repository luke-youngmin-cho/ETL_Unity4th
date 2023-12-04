using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Animations
{
    public class StateMachineBase : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            animator.SetBool("isDirty", false);
        }
    }
}