using UnityEngine;
using UnityEngine.AI;

namespace RPG.AISystems.BT
{
    public class Seek : Node
    {
        public Seek(Tree tree, float distanceLimit) : base(tree)
        {
            _distanceLimit = distanceLimit;
        }

        private float _distanceLimit;

        public override Result Invoke()
        {
            if (blackboard.target &&
                Vector3.Distance(blackboard.transform.position, blackboard.target.position) <= _distanceLimit)
            {
                if (NavMesh.SamplePosition(blackboard.target.position, 
                                           out NavMeshHit hit,
                                           1.0f,
                                           NavMesh.AllAreas))
                {
                    blackboard.agent.isStopped = false;
                    blackboard.agent.SetDestination(hit.position);
                    return Result.Running;
                }
            }

            blackboard.target = null;
            blackboard.agent.isStopped = true;
            return Result.Failure;
        }
    }
}