using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Idle : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Idle;


        public Idle(StateMachine<CharacterStateID> machine) 
            : base(machine)
        {
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            animator.Play("Idle");
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            if (Mathf.Abs(controller.horizontal) > 0.0f)
                nextID = CharacterStateID.Move;

            return nextID;
        }

    }
}
