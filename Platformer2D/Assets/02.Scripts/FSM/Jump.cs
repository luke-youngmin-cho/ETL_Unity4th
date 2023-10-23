using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Jump : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Jump;
        public override bool canExecute => base.canExecute &&
                                           controller.hasJumped == false &&
                                           machine.currentStateID == CharacterStateID.WallSlide ||
                                           ((machine.currentStateID == CharacterStateID.Idle ||
                                             machine.currentStateID == CharacterStateID.Move) &&
                                            controller.isGrounded);

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
            controller.hasJumped = true;
            controller.hasDoubleJumped = false;
            animator.Play("Jump");
            float velocityX = (machine.previousStateID == CharacterStateID.WallSlide) ?
                (controller.horizontal * controller.moveSpeed) : (rigidbody.velocity.x);

            rigidbody.velocity = new Vector2(velocityX, 0.0f);
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
