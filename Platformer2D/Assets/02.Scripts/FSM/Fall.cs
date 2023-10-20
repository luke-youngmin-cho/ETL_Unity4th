using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Fall : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Fall;
        public override bool canExecute => base.canExecute &&
                                           (machine.currentStateID == CharacterStateID.Idle ||
                                            machine.currentStateID == CharacterStateID.Move ||
                                            machine.currentStateID == CharacterStateID.Jump ||
                                            machine.currentStateID == CharacterStateID.DoubleJump ||
                                            machine.currentStateID == CharacterStateID.DownJump);

        private float _landingDistance;
        private float _fallStartPosY;

        public Fall(CharacterMachine machine, float landingDistance)
            : base(machine)
        {
            _landingDistance = landingDistance;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChangeable = true;
            controller.isMovable = false;
            _fallStartPosY = transform.position.y;
            animator.Play("Fall");
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            if (controller.isGrounded)
            {
                nextID = _fallStartPosY - transform.position.y >= _landingDistance ?
                    CharacterStateID.Land : CharacterStateID.Idle;
            }

            return nextID;
        }
    }
}
