using UnityEngine;

namespace RPG.AISystems.BT
{
    public enum Result
    {
        Failure,
        Success,
        Running
    }


    public abstract class Node
    {
        //=====================================================
        //                  Constructors
        //=====================================================

        public Node(Tree tree)
        {
            this.tree = tree;
            this.blackboard = tree.blackboard;
        }


        //=====================================================
        //                  Fields
        //=====================================================

        protected Tree tree;
        protected Blackboard blackboard;


        //=====================================================
        //                  Public Methods
        //=====================================================

        public abstract Result Invoke();
    }
}