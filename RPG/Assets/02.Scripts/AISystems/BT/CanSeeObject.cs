using UnityEngine;

namespace RPG.AISystems.BT
{
    public class CanSeeObject : Node
    {
        public CanSeeObject(Tree tree, float radius, float angle, LayerMask targetMask, Vector3 offset) : base(tree)
        {
            _radius = radius;
            _angle = angle;
            _targetMask = targetMask;
            _offset = offset;
        }

        private float _radius;
        private float _angle;
        private LayerMask _targetMask;
        private Vector3 _offset;


        public override Result Invoke()
        {
            Collider[] cols =
                Physics.OverlapCapsule(blackboard.transform.position,
                                       blackboard.transform.position + _offset,
                                       _radius,
                                       _targetMask);

            if (cols.Length > 0)
            {
                if (IsInSight(cols[0].transform.position))
                {
                    blackboard.target = cols[0].transform;
                    return Result.Success;
                }
            }

            return Result.Failure;
        }

        private bool IsInSight(Vector3 target)
        {
            Vector3 origin = blackboard.transform.position;
            Vector3 forward = blackboard.transform.forward;
            Vector3 lookDir = (target - origin).normalized;
            float theta = Mathf.Acos(Vector3.Dot(forward, lookDir)) * Mathf.Rad2Deg;
            if (theta < _angle / 2.0f )
            {
                if (Physics.Raycast(origin + _offset / 2.0f,
                                    lookDir,
                                    out RaycastHit hit,
                                    Vector3.Distance(target, origin),
                                    _targetMask))
                {
                    return true;
                }
            }

            return false;
        }
    }
}