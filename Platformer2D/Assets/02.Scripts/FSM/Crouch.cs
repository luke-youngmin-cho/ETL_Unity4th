using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Crouch : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Crouch;
        public override bool canExecute => base.canExecute &&
                                           (machine.currentStateID == CharacterStateID.Idle ||
                                            machine.currentStateID == CharacterStateID.Move) &&
                                            controller.isGrounded;

        private int _step;

        public Crouch(CharacterMachine machine)
            : base(machine)
        {
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChangeable = false;
            controller.isMovable = false;
            controller.Stop();
            animator.Play("CrouchStart");
            _step = 0;
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            switch (_step)
            {
                case 0:
                    {
                        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                        {
                            animator.Play("CrouchIdle");
                            _step++;
                        }
                    }
                    break;
                case 1:
                    {
                        // nothing to do
                    }
                    break;
                default:
                    break;
            }

            return nextID;
        }
    }
}
