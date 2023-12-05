
using System;

namespace RPG.AISystems.BT
{
    public class Condition : Node, IParentOfChild
    {
        public Condition(Tree tree, Func<bool> match) : base(tree)
        {
            _match = match;
        }

        public Node child { get; set; }

        private Func<bool> _match;

        public override Result Invoke()
        {
            if (_match.Invoke())
            {
                Result result = child.Invoke();
                return result;
            }

            return Result.Failure;
        }
    }
}