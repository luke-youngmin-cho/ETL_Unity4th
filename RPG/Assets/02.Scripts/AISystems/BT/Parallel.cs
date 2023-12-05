namespace RPG.AISystems.BT
{
    /// <summary>
    /// �ڽĵ��� ���ʴ�� ��ȸ�ϸ鼭, 
    /// ���߿� Running ��ȯ�Ǹ� Running ��ȯ.
    /// �ڽĵ��� ��� ����� �����ؼ� ��å�� ���� ����� ��ȯ
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