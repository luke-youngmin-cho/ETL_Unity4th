using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Idle : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Idle;

        // 기반타입이 생성자 오버로드를 가지면,
        // 하위타입에서 해당 오버로드에 인자를 전달할 수 있도록 파라미터들을 가지는 오버로드가 필요하다.
        // (최소 한개)
        public Idle(CharacterMachine machine)
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
            animator.Play("Idle");
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            if (Mathf.Abs(controller.horizontal) > 0.0f)
                nextID = CharacterStateID.Move;

            if (controller.isGrounded == false)
                nextID = CharacterStateID.Fall;

            return nextID;
        }

    }
}
