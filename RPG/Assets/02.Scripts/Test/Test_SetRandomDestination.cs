using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test_SetRandomDestination : MonoBehaviour
{
    [SerializeField] NavMeshAgent _agent;

    private void OnGUI()
    {
        if (GUILayout.Button("Set Ramdom Destination"))
        {
            Vector3 randomPos = _agent.transform.position + Random.insideUnitSphere * 5.0f;
            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                _agent.SetDestination(hit.position);
            }
        }
    }
}
