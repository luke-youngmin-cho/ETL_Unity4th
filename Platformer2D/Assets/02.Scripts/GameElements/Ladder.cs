using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.GameElements
{
    public class Ladder : MonoBehaviour
    {
        public Vector2 top => (Vector2)transform.position +
                              _bound.offset +
                              Vector2.up * _bound.size.y / 2.0f;
        public Vector2 bottom => (Vector2)transform.position +
                                 _bound.offset +
                                 Vector2.down * _bound.size.y / 2.0f;


        public Vector2 upEnter => bottom +
                                  Vector2.down * _upEnterOffset;

        public Vector2 upExit => top +
                                 Vector2.down * _upExitOffset;

        public Vector2 downEnter => top +
                                    Vector2.down * _downEnterOffset;

        public Vector2 downExit => bottom +
                                   Vector2.down * _downExitOffset;

        [SerializeField] private float _upEnterOffset = 0.03f;
        [SerializeField] private float _upExitOffset = 0.03f;
        [SerializeField] private float _downEnterOffset = 0.05f;
        [SerializeField] private float _downExitOffset = 0.05f;
        private BoxCollider2D _bound;

        private void Awake()
        {
            _bound = GetComponent<BoxCollider2D>();
        }

        private void OnDrawGizmos()
        {
            _bound = GetComponent<BoxCollider2D>();
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(upEnter + Vector2.left * 0.08f,
                            upEnter + Vector2.right * 0.08f);
            Gizmos.DrawLine(upExit + Vector2.left * 0.08f,
                            upExit + Vector2.right * 0.08f);

            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(downEnter + Vector2.left * 0.08f,
                            downEnter + Vector2.right * 0.08f);
            Gizmos.DrawLine(downExit + Vector2.left * 0.08f,
                            downExit + Vector2.right * 0.08f);
        }
    }
}