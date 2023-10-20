using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Move : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Move;


        public Move(CharacterMachine machine) 
            : base(machine)
        {
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChangeable = true;
            controller.isMovable = true;
            controller.hasJumped = false;
            controller.hasDoubleJumped = false;
            animator.Play("Move");
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            if (controller.horizontal == 0.0f)
                nextID = CharacterStateID.Idle;

            if (controller.isGrounded == false)
                nextID = CharacterStateID.Fall;

            return nextID;
        }

    }
}
