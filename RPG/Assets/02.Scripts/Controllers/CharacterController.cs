using RPG.Animations;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Controllers
{
    public enum State
    {
        None,
        Locomotion,
        Jump,
        Fall,
    }

    [RequireComponent(typeof(Rigidbody))]
    public class CharacterController : MonoBehaviour
    {
        public bool aiOn
        {
            get => _aiOn;
            set
            {
                if (_aiOn == value)
                    return;

                _aiOn = value;
                _agent.enabled = value;

                if (value)
                    _agent.speed = moveGain;
            }
        }

        public State state;
        public virtual float horizontal { get; set; }
        public virtual float vertical { get; set; }
        public virtual float moveGain
        {
            get => _moveGain;
            set
            {
                _moveGain = value;

                if (_aiOn)
                    _agent.speed = value;
            }
        }

        public virtual Vector3 velocity 
        { 
            get => _velocity;
            set => _velocity = value;
        }

        private bool _aiOn;
        private float _moveGain;
        private Vector3 _velocity;
        private Vector3 _accel;
        [SerializeField] private float _slopeAngle = 45.0f;
        [SerializeField] private LayerMask _groundMask;
        private Animator _animator;
        private Rigidbody _rigidbody;
        private NavMeshAgent _agent;

        // F = m a  (힘 = 질량 x 가속도)
        public void AddForce(Vector3 force, ForceMode forceMode)
        {
            switch (forceMode)
            {
                case ForceMode.Force:
                    _accel += force / _rigidbody.mass;
                    break;
                case ForceMode.Acceleration:
                    _accel += force;
                    break;
                case ForceMode.Impulse:
                    _velocity += force / _rigidbody.mass;
                    break;
                case ForceMode.VelocityChange:
                    _velocity += force;
                    break;
                default:
                    break;
            }
        }

        public bool ChangeState(State newState)
        {
            _animator.SetInteger("state", (int)newState);
            _animator.SetBool("isDirty", true);
            state = newState;
            return true;
        }

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            _agent = GetComponent<NavMeshAgent>();
            Skill[] skills = _animator.GetBehaviours<Skill>();
            for (int i = 0; i < skills.Length; i++)
            {
                skills[i].Init(this);
            }
        }

        protected virtual void Update()
        {
            if (IsGrounded())
                velocity = new Vector3(horizontal, 0.0f, vertical).normalized * moveGain;

            if (_aiOn)
            {
                horizontal = Vector3.Dot(transform.right, _agent.velocity);
                vertical = Vector3.Dot(transform.forward, _agent.velocity);
            }

            _animator.SetFloat("h", horizontal * moveGain);
            _animator.SetFloat("v", vertical * moveGain);
        }

        private void FixedUpdate()
        {
            if (_aiOn)
            {

            }
            else
            {
                ManualMove();
            }
        }

        public bool IsGrounded()
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, 0.15f, _groundMask);
            return cols.Length > 0;
        }

        private void ManualMove()
        {
            RaycastHit hit;
            NavMeshHit navMeshHit;
            Vector3 expected = transform.position
                               + Quaternion.LookRotation(transform.forward) * _velocity * Time.fixedDeltaTime;

            Debug.DrawRay(transform.position,
                          expected - transform.position, Color.yellow, 0.1f);
            if (Physics.Raycast(transform.position,
                                (expected - transform.position).normalized,
                                out hit,
                                Vector3.Distance(transform.position, expected),
                                _groundMask))
            {
                if (NavMesh.SamplePosition(hit.point,
                                           out navMeshHit,
                                           1.0f,
                                           NavMesh.AllAreas))
                {
                    transform.position = navMeshHit.position;
                }
            }
            else
            {
                transform.position = expected;
            }


            if (IsGrounded())
            {
                _accel.y = .0f;
                _velocity.y = .0f;
                expected = transform.position
                               + Quaternion.LookRotation(transform.forward) * _velocity * Time.fixedDeltaTime;

                float distance = Vector3.Distance(expected, transform.position);
                float slopeHeight = Mathf.Abs(distance * Mathf.Tan(Mathf.Rad2Deg * _slopeAngle));

                Debug.DrawRay(expected + Vector3.up * slopeHeight,
                              Vector3.down * 2 * slopeHeight,
                              Color.red,
                              1.0f);

                Debug.Log($"{velocity}, {slopeHeight}");
                if (Physics.Raycast(expected + Vector3.up * slopeHeight,
                                    Vector3.down,
                                    out hit,
                                    2 * slopeHeight,
                                    _groundMask))
                {
                    if (NavMesh.SamplePosition(hit.point,
                                           out navMeshHit,
                                           1.0f,
                                           NavMesh.AllAreas))
                    {
                        transform.position = navMeshHit.position;
                    }
                }
            }
            else
            {
                _velocity += _accel * Time.fixedDeltaTime;
                _accel += Physics.gravity * Time.fixedDeltaTime;
            }
        }

        private void FootL() { }
        private void FootR() { }
        private void Hit() { }
    }
}