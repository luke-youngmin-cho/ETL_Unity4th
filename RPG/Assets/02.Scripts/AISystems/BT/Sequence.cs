namespace RPG.AISystems.BT
{
    /// <summary>
    /// 자식들을 차례대로 순회하면서, 
    /// 자식이 Failure/Running 을 반환시 해당 결과 반환.
    /// 모두 성공시 Success 반환.
    /// </summary>
    public class Sequence : Composite
    {
        public Sequence(Tree tree) : base(tree)
        {
        }

        public override Result Invoke()
        {
            Result result = Result.Success;

            for (int i = currentSiblingIndex; i < children.Count; i++)
            {
                result = children[i].Invoke();

                switch (result)
                {
                    case Result.Success:
                        {
                            currentSiblingIndex++;
                        }
                        break;
                    case Result.Failure:
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