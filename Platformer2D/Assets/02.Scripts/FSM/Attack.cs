using Platformer.Animations;
using Platformer.Stats;
using Platformer.Datum;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

namespace Platformer.FSM.Character
{
    public class Attack : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Attack;
        public override bool canExecute
        {
            get
            {
                if (base.canExecute == false)
                    return false;

                // 콤보스택이 쌓여있는 상황에서
                // 공격이 끝났던 시간부터 경과된 시간이 콤보 초기화시간을 넘어갔으면 콤보 안됨
                float elapsedTime = Time.time - _exitTimeMark; // 마지막 공격이후 경과된 시간
                if (_comboStack > 0 &&
                    elapsedTime >= _comboResetTime)
                {
                    _comboStack = 0;
                    return false;
                }

                // 현재 스택이 최대치를 넘으면 콤보 안됨
                if (_comboStack > _comboStackMax)
                {
                    return false;
                }

                // 첫타는 무조건 ㅇㅋ 
                // 후속타는 이전 공격 히트판정 이후 ㅇㅋ
                if ((_comboStack == 0 || (_comboStack > 0 && _hasHit)) &&
                    (machine.currentStateID == CharacterStateID.Idle ||
                     machine.currentStateID == CharacterStateID.Move ||
                     machine.currentStateID == CharacterStateID.Crouch ||
                     machine.currentStateID == CharacterStateID.Jump ||
                     machine.currentStateID == CharacterStateID.DownJump ||
                     machine.currentStateID == CharacterStateID.DoubleJump ||
                     machine.currentStateID == CharacterStateID.Fall))
                {
                    return true;
                }

                return false;
            }
        }

        private int _comboStackMax; // 최대 콤보 스택
        private int _comboStack; // 현재 콤보 스택
        private float _comboResetTime; // 공격 이후 콤보 초기화 시간
        private float _exitTimeMark; // 마지막 공격 끝난 시간
        private bool _hasHit; // 현재 공격 히트판정 됐는지 ?


        private SkillCastSetting[] _attackSettings;
        private List<IHp> _targets = new List<IHp>();
        private CharacterAnimationEvents _animationEvents;

        public Attack(CharacterMachine machine, float comboResetTime, SkillCastSetting[] attackSettings) 
            : base(machine)
        {
            _attackSettings = attackSettings;
            _comboStackMax = attackSettings.Length - 1;
            _comboResetTime = comboResetTime;

            _animationEvents = animator.GetComponent<CharacterAnimationEvents>();
            _animationEvents.onHit = () =>
            {
                foreach (var target in _targets)
                {
                    if (target == null)
                        continue;

                    float damage = Random.Range(controller.damageMin, controller.damageMax) * _attackSettings[_comboStack - 1].damageGain;
                    target.DepleteHp(transform, damage);
                }
                _hasHit = true;
            };
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChangeable = false;
            controller.isMovable = false; //controller.isGrounded;
            _hasHit = false;

            SkillCastSetting setting = _attackSettings[_comboStack];
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

            animator.SetFloat("comboStack", _comboStack++); //애니메이션 파라미터 세팅 및 콤보스택 쌓기
            animator.Play("Attack");

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
            _exitTimeMark = Time.time;
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                nextID = CharacterStateID.Idle;

            if (controller.isGrounded)
                controller.move = new Vector2(controller.horizontal * 0.1f, 0.0f);

            return nextID;
        }
    }
}