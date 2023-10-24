using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Hurt : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Hurt;
        public override bool canExecute => base.canExecute &&
                                           (machine.currentStateID == CharacterStateID.Idle ||
                                            machine.currentStateID == CharacterStateID.Move ||
                                            machine.currentStateID == CharacterStateID.Jump ||
                                            machine.currentStateID == CharacterStateID.DownJump ||
                                            machine.currentStateID == CharacterStateID.DoubleJump ||
                                            machine.currentStateID == CharacterStateID.Fall ||
                                            machine.currentStateID == CharacterStateID.Land ||
                                            machine.currentStateID == CharacterStateID.Crouch ||
                                            machine.currentStateID == CharacterStateID.WallSlide ||
                                            machine.currentStateID == CharacterStateID.Dash);


        public Hurt(CharacterMachine machine)
            : base(machine)
        {
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChangeable = false;
            controller.isMovable = false;
            controller.Stop();
            animator.Play("Hurt");//아픔 ㅜㅜ
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                nextID = CharacterStateID.Idle;

            return nextID;
        }
    }
}
