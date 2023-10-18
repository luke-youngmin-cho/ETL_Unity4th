using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Move : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Move;


        public Move(StateMachine<CharacterStateID> machine) 
            : base(machine)
        {
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            animator.Play("Move");
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            if (controller.horizontal == 0.0f)
                nextID = CharacterStateID.Idle;

            return nextID;
        }

    }
}
