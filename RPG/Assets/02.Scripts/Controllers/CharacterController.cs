using RPG.Animations;
using UnityEngine;

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
        public State state;
        public virtual float horizontal { get; set; }
        public virtual float vertical { get; set; }
        public virtual float moveGain { get; set; }

        public virtual Vector3 velocity 
        { 
            get => _velocity;
            set => _velocity = value;
        }

        private Vector3 _velocity;
        private Vector3 _accel;
        [SerializeField] private float _slopeAngle = 45.0f;
        [SerializeField] private LayerMask _groundMask;
        private Animator _animator;
        private Rigidbody _rigidbody;
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

            _animator.SetFloat("h", horizontal * moveGain);
            _animator.SetFloat("v", vertical * moveGain);
        }

        private void FixedUpdate()
        {
            transform.position += _velocity * Time.fixedDeltaTime;

            if (IsGrounded())
            {
                _accel = Vector3.zero;
                _velocity = Vector3.zero;
                Vector3 expected = transform.position
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
                                    out RaycastHit hit,
                                    2 * slopeHeight,
                                    _groundMask))
                {
                    transform.position = hit.point;
                }
            }
            else
            {
                _velocity += _accel * Time.fixedDeltaTime;
                _accel += Physics.gravity * Time.fixedDeltaTime;
            }
        }

        private bool IsGrounded()
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, 0.01f, _groundMask);
            return cols.Length > 0;
        }


        private void FootL() { }
        private void FootR() { }
        private void Hit() { }
    }
}