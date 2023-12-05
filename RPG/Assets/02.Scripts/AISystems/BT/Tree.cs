using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.AISystems.BT
{
    public class Tree : MonoBehaviour
    {
        public Blackboard blackboard;
        public Stack<Node> stack;
        public Root root;
        public bool isBusy;

        private void Start()
        {
            blackboard = new Blackboard(this);
            root = new Root(this);
        }

        private void Update()
        {
            if (isBusy == false)
            {
                isBusy = true;
                StartCoroutine(C_Tick());
            }
        }

        private IEnumerator C_Tick()
        {
            stack.Clear();
            stack.Push(root);

            while (stack.Count > 0)
            {
                Node current = stack.Pop();
                Result result = current.Invoke();

                if (result == Result.Running)
                {
                    stack.Push(current);
                    yield return null;
                }
            }
            isBusy = false;
        }

        #region Builder
        private Node _current;
        private Stack<Composite> _compositeStack;

        public Tree StartBuild()
        {
            _current = root;
            _compositeStack = new Stack<Composite>();
            return this;
        }

        public Tree Selector()
        {
            Composite node = new Selector(this);
            Attach(_current, node);
            _compositeStack.Push(node);
            _current = node;
            return this;
        }

        public Tree Sequence()
        {
            Composite node = new Sequence(this);
            Attach(_current, node);
            _compositeStack.Push(node);
            _current = node;
            return this;
        }

        public Tree Execution(Func<Result> execute)
        {
            Node node = new Execution(this, execute);
            Attach(_current, node);
            _current = _compositeStack.Count > 0 ? _compositeStack.Peek() : null;
            return this;
        }

        public Tree ExitCurrentComposite()
        {
            if (_compositeStack.Count > 0)
            {
                _compositeStack.Pop();
                _current = _compositeStack.Count > 0 ? _compositeStack.Peek() : null;
            }
            else
            {
                throw new Exception($"[Tree] : Composite stack is empty");
            }
            return this;
        }

        private void Attach(Node parent, Node child)
        {
            if (parent is IParentOfChild)
            {
                ((IParentOfChild)parent).child = child;
            }
            else if(parent is IParentOfChildren)
            {
                ((IParentOfChildren)parent).children.Add(child);
            }
            else
            {
                throw new System.Exception($"[Tree] : {parent.GetType()} has no child");
            }
        }


        #endregion
    }
}