namespace RPG.AISystems.BT
{
    /// <summary>
    /// 자식들을 차례대로 순회하면서, 
    /// 자식이 Success/Running 을 반환시 해당 결과 반환.
    /// 모두 실패시 Failure 반환.
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