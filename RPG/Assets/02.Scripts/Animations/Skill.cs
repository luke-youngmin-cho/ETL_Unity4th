using RPG.Controllers;
using UnityEngine;
using CharacterController = RPG.Controllers.CharacterController;

namespace RPG.Animations
{
    public class Skill : StateMachineBase
    {
        [SerializeField] private State _destination;
        protected CharacterController controller;

        public void Init(CharacterController controller)
        {
            this.controller = controller;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (stateInfo.normalizedTime >= 1.0f)
            {
                controller.ChangeState(_destination);
            }
        }
    }
}