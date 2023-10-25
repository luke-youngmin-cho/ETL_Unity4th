using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Slide : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Slide;
        public override bool canExecute => base.canExecute &&
                                           (machine.currentStateID == CharacterStateID.Idle ||
                                            machine.currentStateID == CharacterStateID.Move ||
                                            machine.currentStateID == CharacterStateID.Crouch) &&
                                           controller.isGrounded;

        private float _distance;
        private Vector3 _startPosition;
        private Vector3 _targetPosition;
        private Vector2 _originalColliderOffset; // 원래 충돌체 오프셋
        private Vector2 _originalColliderSize; // 원래 충돌체 크기
        private Vector2 _crouchedColliderOffset; // 숙인상태의 충돌체 오프셋
        private Vector2 _crouchedColliderSize; // 숙인상태의 충돌체 크기

        public Slide(CharacterMachine machine, float distance, Vector2 crouchedColliderOffset, Vector2 crouchedColliderSize)
            : base(machine)
        {
            _distance = distance;
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
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
            _startPosition = transform.position;
            _targetPosition = transform.position + Vector3.right * controller.direction * _distance;
            collision.offset = trigger.offset = _crouchedColliderOffset;
            collision.size = trigger.size = _crouchedColliderSize;
            animator.Play("Slide");
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            collision.offset = trigger.offset = _originalColliderOffset;
            collision.size = trigger.size = _originalColliderSize;
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            float t = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            t = Mathf.Log10(10 + t * 90) - 1;
            Debug.Log(t);
            Vector3 expected
                = Vector3.Lerp(_startPosition, _targetPosition, t);

            bool isValid = true;
            // todo -> expected 위치가 유효한지 확인 (맵의 경계를 벗어난다든지, 벽이 있다든지 등.. )
            if (Physics2D.OverlapCapsule((Vector2)expected + trigger.offset,
                                         trigger.size,
                                         trigger.direction,
                                         0.0f,
                                         1 << LayerMask.NameToLayer("Wall")))
            {
                _startPosition = transform.position;
                _targetPosition = transform.position;
                isValid = false;
            }

            if (isValid)
                transform.position = expected;

            if (t >= 1.0f)
                nextID = CharacterStateID.Idle;

            return nextID;
        }
    }
}
