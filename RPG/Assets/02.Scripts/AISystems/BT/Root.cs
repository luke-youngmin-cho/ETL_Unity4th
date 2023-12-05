namespace RPG.AISystems.BT
{
    public class Root : Node, IParentOfChild
    {
        public Root(Tree tree) : base(tree)
        {
        }

        public Node child { get; set; }

        public override Result Invoke()
        {
            tree.stack.Push(child);
            return Result.Success;
        }
    }
}