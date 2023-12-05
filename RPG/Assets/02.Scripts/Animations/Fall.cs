using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Animations
{
    public class Fall : Skill
    {
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
            if (controller.IsGrounded())
                controller.ChangeState(Controllers.State.Locomotion);
        }
    }
}