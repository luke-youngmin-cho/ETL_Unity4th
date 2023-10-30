using Platformer.Effects;
using Platformer.FSM;
using Platformer.GameElements;
using Platformer.Stats;
using System;
using System.Linq;
using UnityEngine;

namespace Platformer.Controllers
{
    public abstract class CharacterController : MonoBehaviour, IHp
    {
        public float damageMin; // 최소공
        public float damageMax; // 최대공

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

                onDirectionChanged?.Invoke(_direction);
            }
        }
        private int _direction;
        public event Action<int> onDirectionChanged;
        public bool isDirectionChangeable;

        public abstract float horizontal { get; }
        public abstract float vertical { get; }

        public bool isMovable;
        public Vector2 move;
        public float moveSpeed => _moveSpeed;
        [SerializeField] private float _moveSpeed;
        protected Rigidbody2D rigidbody;

        #region Ground Detection
        public bool isGrounded
        {
            get
            {
                ground = Physics2D.OverlapBox(rigidbody.position + _groundDetectOffset,
                                              _groundDetectSize,
                                              0.0f,
                                              groundMask);
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
                                         layerMask: groundMask);

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
        [SerializeField] protected LayerMask groundMask;
        #endregion

        #region Wall Detection
        public bool isWallDetected
        {
            get
            {
                RaycastHit2D topHit = Physics2D.Raycast(wallTopCastCenter, Vector2.right * _direction, _wallDetectDistance, _wallMask);
                RaycastHit2D bottomHit = Physics2D.Raycast(wallBottomCastCenter, Vector2.right * _direction, _wallDetectDistance, _wallMask);
                return topHit.collider && bottomHit.collider;
            }
        }
        private Vector2 wallTopCastCenter => rigidbody.position + Vector2.up * _col.size.y;
        private Vector2 wallBottomCastCenter => rigidbody.position;
        [SerializeField] private LayerMask _wallMask;
        [SerializeField] private float _wallDetectDistance;
        #endregion

        #region Ladder Detection

        public bool isUpLadderDetected
        {
            get
            {
                Collider2D col = Physics2D.OverlapCircle(transform.position + Vector3.up * _ladderUpDetectOffset,
                                                         _ladderDetectRadius,
                                                         _ladderMask);

                if (col)
                {
                    if (col.TryGetComponent(out upLadder))
                    {
                        return true;
                    }
                    else
                    {
                        throw new Exception($"[CharacterController] : Seems like layer of {col.name} is wrong");
                    }
                }

                upLadder = null;
                return false;
            }
        }

        public bool isDownLadderDetected
        {
            get
            {
                Collider2D col = Physics2D.OverlapCircle(transform.position + Vector3.down * _ladderDownDetectOffset,
                                                         _ladderDetectRadius,
                                                         _ladderMask);

                if (col)
                {
                    downLadder = col.GetComponent<Ladder>();
                    return true;
                }

                downLadder = null;
                return false;
            }
        }

        public Ladder upLadder;
        public Ladder downLadder;
        [SerializeField] private float _ladderUpDetectOffset; // 사다리타고 올라가기위한 위치감지 오프셋
        [SerializeField] private float _ladderDownDetectOffset; // 사다리타고 내려가기위한 위치감지 오프셋
        [SerializeField] private float _ladderDetectRadius; // 감지 반경
        [SerializeField] private LayerMask _ladderMask;

        #endregion

        public float hpValue
        {
            get => _hp;
            set
            {
                if (value == _hp)
                    return;

                value = Mathf.Clamp(value, hpMin, hpMax);
                _hp = value;

                onHpChanged?.Invoke(value);

                if (value == hpMax)
                    onHpMax?.Invoke();
                else if (value == hpMin)
                    onHpMin?.Invoke();
            }
        }

        public float hpMax => _hpMax;

        public float hpMin => 0f;

        public bool invincible { get; set; }

        private float _hp;

        [SerializeField] private float _hpMax;
        public event Action<float> onHpChanged;
        public event Action<float> onHpRecovered;
        public event Action<float> onHpDepleted;
        public event Action onHpMax;
        public event Action onHpMin;


        private CapsuleCollider2D _col;


        public bool hasJumped;
        public bool hasDoubleJumped;
        protected CharacterMachine machine;


        public virtual void SetUp()
        {
            hpValue = hpMax;
            var renderer = GetComponentInChildren<SpriteRenderer>();
            Color color = renderer.color;
            color.a = 1.0f;
            renderer.color = color;
        }

        public void Knockback(Vector2 force)
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.AddForce(force, ForceMode2D.Impulse);
        }

        public void Stop()
        {
            move = Vector2.zero; // 입력 0
            rigidbody.velocity = Vector2.zero; // 속도 0
        }

        protected virtual void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            _col = GetComponent<CapsuleCollider2D>();
            hpValue = hpMax;
        }

        protected virtual void Start()
        {
            
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

        protected virtual void OnDrawGizmosSelected()
        {
            DrawGroundDetectGizmos();
            DrawGroundBelowDetectGizmos();
            DrawWallDetectGizmos();
            DrawLadderDetectGizmos();
        }

        private void DrawGroundDetectGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + (Vector3)_groundDetectOffset, _groundDetectSize);
        }

        private void DrawGroundBelowDetectGizmos()
        {
            Vector3 castStartPos = transform.position + (Vector3)_groundDetectOffset + Vector3.down * _groundDetectSize.y + Vector3.down * 0.01f;
            RaycastHit2D[] hits =
                Physics2D.BoxCastAll(origin: castStartPos,
                                     size: _groundDetectSize,
                                     angle: 0.0f,
                                     direction: Vector2.down,
                                     distance: _groundBelowDetectDistance,
                                     layerMask: groundMask);

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

        private void DrawWallDetectGizmos()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            _col = GetComponent<CapsuleCollider2D>();
            _direction = (int)Mathf.Sign(transform.localScale.x);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(wallTopCastCenter, wallTopCastCenter + Vector2.right * _wallDetectDistance * _direction);
            Gizmos.DrawLine(wallBottomCastCenter, wallBottomCastCenter + Vector2.right * _wallDetectDistance * _direction);
        }

        private void DrawLadderDetectGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(transform.position + Vector3.up * _ladderUpDetectOffset, _ladderDetectRadius);
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(transform.position + Vector3.down * _ladderDownDetectOffset, _ladderDetectRadius);
        }

        public void RecoverHp(object subject, float amount)
        {
            hpValue += amount;
            onHpRecovered?.Invoke(amount);
        }

        public virtual void DepleteHp(object subject, float amount)
        {
            hpValue -= amount;
            onHpDepleted?.Invoke(amount);            
        }
    }
}