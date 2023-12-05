namespace RPG.AISystems.BT
{
    /// <summary>
    /// �ڽĵ��� ���ʴ�� ��ȸ�ϸ鼭, 
    /// �ڽ��� Success/Running �� ��ȯ�� �ش� ��� ��ȯ.
    /// ��� ���н� Failure ��ȯ.
    /// </summary>
    public class Selector : Composite
    {
        public Selector(Tree tree) : base(tree)
        {
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
                        {
                            currentSiblingIndex++;
                        }
                        break;
                    case Result.Success:
                        {
                            currentSiblingIndex = 0;
                            return result;
                        }
                    case Result.Running:
                        {
                            return result;
                        }
                    default:
                        break;
                }
            }

            currentSiblingIndex = 0;
            return result;
        }
    }
}