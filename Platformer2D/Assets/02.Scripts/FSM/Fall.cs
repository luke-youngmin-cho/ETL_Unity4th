using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Fall : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Fall;
        public override bool canExecute => base.canExecute &&
                                           (machine.currentStateID == CharacterStateID.Idle ||
                                            machine.currentStateID == CharacterStateID.Move ||
                                            machine.currentStateID == CharacterStateID.Jump);


        public Fall(CharacterMachine machine)
            : base(machine)
        {
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChangeable = true;
            controller.isMovable = false;
            animator.Play("Fall");
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            if (controller.isGrounded)
                nextID = CharacterStateID.Idle;

            return nextID;
        }
    }
}
