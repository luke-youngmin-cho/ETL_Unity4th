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

        protected virtual void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (isMovable)
            {
                move = new Vector2(horizontal * _moveSpeed, 0.0f);
            }

            if (Mathf.Abs(horizontal) > 0.0f)
            {
                direction = horizontal < 0.0f ? DIRECTION_LEFT : DIRECTION_RIGHT;
            }
        }

        protected virtual void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            rigidbody.position += move * Time.fixedDeltaTime;
        }
    }
}