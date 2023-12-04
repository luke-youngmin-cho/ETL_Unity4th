using UnityEngine;

namespace RPG.Controllers
{
    public class CharacterController : MonoBehaviour
    {
        public virtual float horizontal { get; set; }
        public virtual float vertical { get; set; }
        public virtual float moveGain { get; set; }

        public virtual Vector3 velocity { get; set; }

        private Vector3 _velocity;
        private Vector3 _accel;
        [SerializeField] private float _slopeAngle = 45.0f;
        [SerializeField] private LayerMask _groundMask;

        private void FixedUpdate()
        {
            if (IsGrounded())
                _accel = Vector3.zero;
            else
                _accel += Physics.gravity * Time.fixedDeltaTime;

            Vector3 expected = transform.position
                               + Quaternion.LookRotation(transform.forward) * _velocity * Time.fixedDeltaTime;

            float distance = Vector3.Distance(expected, transform.position);
            float slopeHeight = distance * Mathf.Tan(Mathf.Rad2Deg * _slopeAngle);

            if (Physics.Raycast(expected + Vector3.up * slopeHeight,
                                Vector3.down,
                                out RaycastHit hit,
                                2 * slopeHeight,
                                _groundMask))
            {
                transform.position = hit.point;
            }
        }

        private bool IsGrounded()
        {
            return true;
        }
    }
}