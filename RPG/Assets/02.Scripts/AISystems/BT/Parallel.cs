namespace RPG.AISystems.BT
{
    /// <summary>
    /// 자식들을 차례대로 순회하면서, 
    /// 도중에 Running 반환되면 Running 반환.
    /// 자식들의 모든 결과를 종합해서 정책에 따라 결과를 반환
    /// </summary>
    public class Parallel : Composite
    {
        public enum Policy
        {
            RequireOne,
            RequireAll
        }
        private Policy _successPolicy;
        private int _successCount;


        public Parallel(Tree tree, Policy successPolicy) : base(tree)
        {
            _successPolicy = successPolicy;
        }

        public override Result Invoke()
        {
            Result result = Result.Failure;

            for (int i = currentSiblingIndex; i < children.Count; i++)
            {
                result = children[i].Invoke();

                switch (result)
                {
                    case Result.Failure:
                        break;
                    case Result.Success:
                        {
                            _successCount++;
                        }
                        break;
                    case Result.Running:
                        {
                            return result;
                        }
                    default:
                        break;
                }
                currentSiblingIndex++;
            }

            switch (_successPolicy)
            {
                case Policy.RequireOne:
                    {
                        result = _successCount > 0 ? Result.Success : Result.Failure;
                    }
                    break;
                case Policy.RequireAll:
                    {
                        result = _successCount == children.Count ? Result.Success : Result.Failure;
                    }
                    break;
                default:
                    throw new System.Exception();
            }

            currentSiblingIndex = 0;
            _successCount = 0;
            return result;
        }
    }
}