using UnityEngine;
using UnityEngine.AI;
using CharacterController = RPG.Controllers.CharacterController;

namespace RPG.AISystems.BT
{
    public class Blackboard
    {
        public Blackboard(Tree tree)
        {
            this.tree = tree;
            this.transform = tree.GetComponent<Transform>();
            this.agent = tree.GetComponent<NavMeshAgent>();
            this.controller = tree.GetComponent<CharacterController>();
        }

        // owner
        public Tree tree;
        public Transform transform;
        public NavMeshAgent agent;
        public CharacterController controller;

        // target
        public Transform target;
    }
}