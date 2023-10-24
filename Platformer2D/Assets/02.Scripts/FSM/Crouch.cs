using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Crouch : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Crouch;
        public override bool canExecute => base.canExecute &&
                                           (machine.currentStateID == CharacterStateID.Idle ||
                                            machine.currentStateID == CharacterStateID.Move) &&
                                            controller.isGrounded;

        private int _step;

        private Vector2 _originalColliderOffset; // 원래 충돌체 오프셋
        private Vector2 _originalColliderSize; // 원래 충돌체 크기
        private Vector2 _crouchedColliderOffset; // 숙인상태의 충돌체 오프셋
        private Vector2 _crouchedColliderSize; // 숙인상태의 충돌체 크기

        public Crouch(CharacterMachine machine, Vector2 crouchedColliderOffset, Vector2 crouchedColliderSize)
            : base(machine)
        {
            _originalColliderOffset = trigger.offset;
            _originalColliderSize = trigger.size;
            _crouchedColliderOffset = crouchedColliderOffset;
            _crouchedColliderSize = crouchedColliderSize;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChangeable = false;
            controller.isMovable = false;
            controller.Stop();
            collision.offset = trigger.offset = _crouchedColliderOffset;
            collision.size = trigger.size = _crouchedColliderSize;
            animator.Play("CrouchStart");
            _step = 0;
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            collision.offset = trigger.offset = _originalColliderOffset;
            collision.size = trigger.size = _originalColliderSize;
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            switch (_step)
            {
                case 0:
                    {
                        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                        {
                            animator.Play("CrouchIdle");
                            _step++;
                        }
                    }
                    break;
                case 1:
                    {
                        // nothing to do
                    }
                    break;
                default:
                    break;
            }

            return nextID;
        }
    }
}
