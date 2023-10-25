using Platformer.GameElements;
using Unity.VisualScripting;
using UnityEngine;

namespace Platformer.FSM
{
    /// <summary>
    /// 아래에 감지된 사다리를 타는 상태
    /// </summary>
    public class DownLadderClimb : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.DownLadderClimb;

        public override bool canExecute => base.canExecute &&
                                           (machine.currentStateID == CharacterStateID.Idle ||
                                            machine.currentStateID == CharacterStateID.Move ||
                                            machine.currentStateID == CharacterStateID.Crouch ||
                                            machine.currentStateID == CharacterStateID.Dash ||
                                            machine.currentStateID == CharacterStateID.Jump ||
                                            machine.currentStateID == CharacterStateID.Fall ||
                                            machine.currentStateID == CharacterStateID.DownJump ||
                                            machine.currentStateID == CharacterStateID.DoubleJump) &&
                                           controller.isDownLadderDetected &&
                                           controller.isGrounded;

        private Ladder _ladder; // 사다리 타는 도중 사다리 감지가 안될 수 있으므로 처음 타기 시작할때 캐싱
        private float _vertical; // 컨트롤러의 수직 입력으로 움직일 변수
        private bool _doExit;

        public DownLadderClimb(CharacterMachine machine) : base(machine)
        {
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChangeable = false;
            controller.isMovable = false;
            controller.hasDoubleJumped = false;
            controller.Stop();
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
            animator.Play("LadderDown");
            animator.speed = 0.0f;

            _ladder = controller.downLadder;
            // 플레이어의 위치가 사다리 아래로타기 진입점보다 낮으면 현재위치 그대로, 높으면 진입점으로 이동
            Vector2 startPos = transform.position.y < _ladder.downEnter.y ? new Vector2(_ladder.top.x, transform.position.y) : _ladder.downEnter;
            transform.position = startPos;
            _doExit = false;
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            animator.speed = 1.0f;
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            // 위쪽으로 방향전환시
            if (_vertical <= 0 && controller.vertical > 0)
            {
                animator.Play("LadderUp");
            }
            // 아래쪽으로 방향전환시
            else if (_vertical >= 0 && controller.vertical < 0)
            {
                animator.Play("LadderDown");
            }

            _vertical = controller.vertical;
            animator.speed = Mathf.Abs(_vertical); // 수직 입력에 따라 애니메이션 재생


            controller.hasJumped = controller.horizontal == 0.0f; // 수평입력 들어올시 점프 가능하도록

            if (_doExit)
                nextID = CharacterStateID.Idle;

            return nextID;
        }

        public override void OnStateFixedUpdate()
        {
            base.OnStateFixedUpdate();

            transform.position += Vector3.up * _vertical * Time.fixedDeltaTime;

            if (transform.position.y >= _ladder.upExit.y)
            {
                transform.position = _ladder.top;
                _doExit = true;
            }
            else if (transform.position.y <= _ladder.downExit.y)
            {
                _doExit = true;
            }
        }
    }
}