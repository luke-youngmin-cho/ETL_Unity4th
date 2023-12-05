using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Animations
{
    public class Jump : Skill
    {
        [SerializeField] private float _force = 5.0f;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            controller.transform.position += Vector3.up * 0.15f;
            controller.AddForce(Vector3.up * _force, ForceMode.Impulse);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //base.OnStateUpdate(animator, stateInfo, layerIndex);
            if (controller.velocity.y <= 0.0f)
                controller.ChangeState(Controllers.State.Fall);
        }
    }
}