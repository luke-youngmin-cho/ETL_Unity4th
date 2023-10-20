using UnityEngine;

namespace Platformer.FSM.Character
{
    public class DoubleJump : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.DoubleJump;
        public override bool canExecute => base.canExecute &&
                                           controller.hasDoubleJumped == false &&
                                           (machine.currentStateID == CharacterStateID.Jump ||
                                            machine.currentStateID == CharacterStateID.Fall);

        private float _jumpForce;

        public DoubleJump(CharacterMachine machine, float jumpForce)
            : base(machine)
        {
            _jumpForce = jumpForce;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChangeable = true;
            controller.isMovable = false;
            controller.hasJumped = true;
            controller.hasDoubleJumped = true;
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
