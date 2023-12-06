using RPG.AISystems.BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tree = RPG.AISystems.BT.Tree;

namespace RPG.Controllers
{
    public class EnemyController : CharacterController
    {
        private Tree _tree;

        [Header("AI")]
        [SerializeField] float _targetDetectRadius;
        [SerializeField] float _targetDetectAngle;
        [SerializeField] LayerMask _targetMask;
        [SerializeField] Vector3 _targetDetectOffset;
        [SerializeField] float _targetFollowDistanceLimit;

        private void Start()
        {
            _tree = gameObject.AddComponent<Tree>();
            _tree.StartBuild()
                .Selector()
                    .Sequence()
                        .CanSeeObject(_targetDetectRadius,
                                      _targetDetectAngle,
                                      _targetMask,
                                      _targetDetectOffset)
                        .Seek(_targetFollowDistanceLimit);


            //_tree.root.child = new Selector(_tree);
            //((Selector)_tree.root.child).children.Add(new Sequence(_tree));
            //((Sequence)((Selector)_tree.root.child).children[0]).children.Add(new Execution(_tree, () => Result.Success));
            aiOn = true;
            moveGain = 2.0f;
        }

        private void OnDrawGizmos()
        {
            DrawArcGizmos(_targetDetectRadius, _targetDetectAngle, Color.yellow);

            if (_tree)
            {
                if (_tree.blackboard.target)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(transform.position, _tree.blackboard.target.position);
                }
            }
        }

        private void DrawArcGizmos(float radius, float angle, Color color)
        {
            Gizmos.color = color;
            Vector3 left = Quaternion.Euler(0f, -angle / 2.0f, 0.0f) * transform.forward;
            Vector3 right = Quaternion.Euler(0f, angle / 2.0f, 0.0f) * transform.forward;

            int segements = 10;
            Vector3 prev = transform.position + left * radius;
            for (int i = 0; i < segements; i++)
            {
                float ratio = (float)(i + 1) / segements;
                float theta = Mathf.Lerp(-angle / 2.0f, angle / 2.0f, ratio);
                Vector3 dir = Quaternion.Euler(0f, theta, 0f) * transform.forward;
                Vector3 next = transform.position + dir * radius;
                Gizmos.DrawLine(prev, next);
                prev = next;
            }
            Gizmos.DrawLine(transform.position, transform.position + left * radius);
            Gizmos.DrawLine(transform.position, transform.position + right * radius);
        }
    }
}