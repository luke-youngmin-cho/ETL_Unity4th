using Platformer.FSM;
using System.Linq;
using UnityEngine;

namespace Platformer.Controllers
{
    public abstract class CharacterController : MonoBehaviour
    {
        public const int DIRECTION_RIGHT = 1;
        public const int DIRECTION_LEFT = -1;
        public int direction
        {
            get => _direction;
            set
            {
                if (isDirectionChangeable == false)
                    return;

                if (value == _direction)
                    return;

                if (value > 0)
                {
                    _direction = DIRECTION_RIGHT;
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                else if (value < 0)
                {
                    _direction = DIRECTION_LEFT;
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                else
                    throw new System.Exception("[CharacterController] : Wrong direction.");
            }
        }
        private int _direction;
        public bool isDirectionChangeable;

        public abstract float horizontal { get; }
        public abstract float vertical { get; }

        public bool isMovable;
        public Vector2 move;
        [SerializeField] private float _moveSpeed;
        protected Rigidbody2D rigidbody;

        public bool isGrounded
        {
            get
            {
                ground = Physics2D.OverlapBox(rigidbody.position + _groundDetectOffset,
                                              _groundDetectSize,
                                              0.0f,
                                              _groundMask);
                return ground;
            }
        }

        public bool isGroundBelowExist
        {
            get
            {
                Vector3 castStartPos = transform.position + (Vector3)_groundDetectOffset + Vector3.down * _groundDetectSize.y + Vector3.down * 0.01f;
                RaycastHit2D[] hits =
                    Physics2D.BoxCastAll(origin: castStartPos,
                                         size: _groundDetectSize,
                                         angle: 0.0f,
                                         direction: Vector2.down,
                                         distance: _groundBelowDetectDistance,
                                         layerMask: _groundMask);

                RaycastHit2D hit = default;
                if (hits.Length > 0)
                    hit = hits.FirstOrDefault(x => ground ?? x != ground);

                groundBelow = hit.collider;
                return groundBelow;
            }
        }

        public Collider2D ground;
        public Collider2D groundBelow;
        [SerializeField] private Vector2 _groundDetectOffset;
        [SerializeField] private Vector2 _groundDetectSize;
        [SerializeField] private float _groundBelowDetectDistance;

        [SerializeField] private LayerMask _groundMask;

        public bool hasJumped;
        public bool hasDoubleJumped;
        protected CharacterMachine machine;

        public void Stop()
        {
            move = Vector2.zero; // 입력 0
            rigidbody.velocity = Vector2.zero; // 속도 0
        }

        protected virtual void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        protected virtual void Update()
        {
            machine.UpdateState();

            if (isMovable)
            {
                move = new Vector2(horizontal * _moveSpeed, 0.0f);
            }

            if (Mathf.Abs(horizontal) > 0.0f)
            {
                direction = horizontal < 0.0f ? DIRECTION_LEFT : DIRECTION_RIGHT;
            }
        }

        protected virtual void LateUpdate()
        {
            machine.LateUpdateState();
        }

        protected virtual void FixedUpdate()
        {
            machine.FixedUpdateState();

            Move();
        }

        private void Move()
        {
            rigidbody.position += move * Time.fixedDeltaTime;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + (Vector3)_groundDetectOffset, _groundDetectSize);

            Vector3 castStartPos = transform.position + (Vector3)_groundDetectOffset + Vector3.down * _groundDetectSize.y + Vector3.down * 0.01f;
            RaycastHit2D[] hits =
                Physics2D.BoxCastAll(origin: castStartPos,
                                     size: _groundDetectSize,
                                     angle: 0.0f,
                                     direction: Vector2.down,
                                     distance: _groundBelowDetectDistance,
                                     layerMask: _groundMask);

            RaycastHit2D hit = default;
            if (hits.Length > 0)
                hit = hits.FirstOrDefault(x => ground ?? x != ground);


            Gizmos.color = Color.gray;
            Gizmos.DrawWireCube(castStartPos, _groundDetectSize);
            Gizmos.DrawWireCube(castStartPos + Vector3.down * _groundBelowDetectDistance, _groundDetectSize);
            Gizmos.DrawLine(castStartPos + Vector3.left * _groundDetectSize.x / 2.0f,
                            castStartPos + Vector3.left * _groundDetectSize.x / 2.0f + Vector3.down * _groundBelowDetectDistance);
            Gizmos.DrawLine(castStartPos + Vector3.right * _groundDetectSize.x / 2.0f,
                            castStartPos + Vector3.right * _groundDetectSize.x / 2.0f + Vector3.down * _groundBelowDetectDistance);

            if (hit.collider != null)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireCube(castStartPos, _groundDetectSize);
                Gizmos.DrawWireCube(castStartPos + Vector3.down * hit.distance, _groundDetectSize);
                Gizmos.DrawLine(castStartPos + Vector3.left * _groundDetectSize.x / 2.0f,
                                castStartPos + Vector3.left * _groundDetectSize.x / 2.0f + Vector3.down * hit.distance);
                Gizmos.DrawLine(castStartPos + Vector3.right * _groundDetectSize.x / 2.0f,
                                castStartPos + Vector3.right * _groundDetectSize.x / 2.0f + Vector3.down * hit.distance);
            }
        }
    }
}