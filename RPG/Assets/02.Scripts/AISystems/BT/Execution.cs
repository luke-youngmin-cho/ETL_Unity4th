using System;

namespace RPG.AISystems.BT
{
    public class Execution : Node
    {
        public Execution(Tree tree, Func<Result> execute) : base(tree)
        {
            _execute = execute;
        }

        private Func<Result> _execute;

        public override Result Invoke()
        {
            return _execute.Invoke();
        }
    }
}