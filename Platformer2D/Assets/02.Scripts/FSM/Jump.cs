using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Jump : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Jump;
        public override bool canExecute => base.canExecute &&
                                           (machine.currentStateID == CharacterStateID.Idle ||
                                            machine.currentStateID == CharacterStateID.Move) &&
                                           controller.isGrounded;

        private float _jumpForce;

        public Jump(CharacterMachine machine, float jumpForce)
            : base(machine)
        {
            _jumpForce = jumpForce;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChangeable = true;
            controller.isMovable = false;
            animator.Play("Jump");
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0.0f);
            rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            if (rigidbody.velocity.y <= 0.0f)
                nextID = CharacterStateID.Fall;

            return nextID;
        }
    }
}
