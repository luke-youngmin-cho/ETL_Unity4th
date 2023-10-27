using Platformer.Animations;
using Platformer.Stats;
using Platformer.Datum;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Platformer.FSM.Character
{
    public class DashAttack : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.DashAttack;
        public override bool canExecute => base.canExecute &&
                                           machine.currentStateID == CharacterStateID.Dash;

        private SkillCastSetting _attackSetting;
        private List<IHp> _targets = new List<IHp>();
        private CharacterAnimationEvents _animationEvents;
        private float _distance;
        private Vector3 _startPosition;
        private Vector3 _targetPosition;

        public DashAttack(CharacterMachine machine, float distance, SkillCastSetting attackSettings) 
            : base(machine)
        {
            _distance = distance;
            _attackSetting = attackSettings;

            _animationEvents = animator.GetComponent<CharacterAnimationEvents>();
            
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChangeable = false;
            controller.isMovable = false; //controller.isGrounded;
            controller.Stop();
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
            _startPosition = transform.position;
            _targetPosition = transform.position + Vector3.right * controller.direction * _distance;

            SkillCastSetting setting = _attackSetting;
            RaycastHit2D[] hits =
                Physics2D.BoxCastAll(origin: rigidbody.position + new Vector2(setting.castCenter.x * controller.direction, setting.castCenter.y),
                                     size: setting.castSize,
                                     angle: 0.0f,
                                     direction: Vector2.right * controller.direction,
                                     distance: setting.castDistance,
                                     layerMask: setting.targetMask);

            // 전체 감지된 아이들중에서 최대 타겟 수 까지만 대상으로 등록
            _targets.Clear();
            for (int i = 0; i < hits.Length; i++)
            {
                if (_targets.Count >= setting.targetMax)
                    break;

                if (hits[i].collider.TryGetComponent(out IHp target))
                    _targets.Add(target);
            }

            _animationEvents.onHit = () =>
            {
                foreach (var target in _targets)
                {
                    if (target == null)
                        continue;

                    float damage = Random.Range(controller.damageMin, controller.damageMax) * _attackSetting.damageGain;
                    target.DepleteHp(transform, damage);
                }
            };
            animator.Play("DashAttack");

            Vector2 origin = rigidbody.position + new Vector2(setting.castCenter.x * controller.direction, setting.castCenter.y);
            Vector2 size = setting.castSize;
            float distance = setting.castDistance;
            // L-T -> R-T
            Debug.DrawLine(origin + new Vector2(-size.x / 2.0f * controller.direction, +size.y / 2.0f),
                           origin + new Vector2(+size.x / 2.0f * controller.direction, +size.y / 2.0f) + Vector2.right * controller.direction * distance,
                           Color.red,
                           animator.GetCurrentAnimatorStateInfo(0).length);
            // L-B -> R-B
            Debug.DrawLine(origin + new Vector2(-size.x / 2.0f * controller.direction, -size.y / 2.0f),
                           origin + new Vector2(+size.x / 2.0f * controller.direction, -size.y / 2.0f) + Vector2.right * controller.direction * distance,
                           Color.red,
                           animator.GetCurrentAnimatorStateInfo(0).length);
            // L-T -> L-B
            Debug.DrawLine(origin + new Vector2(-size.x / 2.0f * controller.direction, +size.y / 2.0f),
                           origin + new Vector2(-size.x / 2.0f * controller.direction, -size.y / 2.0f),
                           Color.red,
                           animator.GetCurrentAnimatorStateInfo(0).length);
            // R-T -> R-B
            Debug.DrawLine(origin + new Vector2(+size.x / 2.0f * controller.direction, +size.y / 2.0f) + Vector2.right * controller.direction * distance,
                           origin + new Vector2(+size.x / 2.0f * controller.direction, -size.y / 2.0f) + Vector2.right * controller.direction * distance,
                           Color.red,
                           animator.GetCurrentAnimatorStateInfo(0).length);

        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                nextID = CharacterStateID.Idle;

            float t = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            t = t < 0.05f ? t / 10.0f : Mathf.Log10(10 + t * 90) - 1;
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