using System.Collections.Generic;

namespace RPG.AISystems.BT
{
    public abstract class Composite : Node, IParentOfChildren
    {
        protected Composite(Tree tree) : base(tree)
        {
            children = new List<Node>();
        }

        // ���������� �����ߴ� �ڽ��� �ε���
        // �̰� ����س��� �ڽ��� Running ���¿��� ����������, 
        // ���������� �ٽ� �� Composite �� �����ؾ� �� �� ù �ڽĺ��� ���������ʰ�
        // ���������� �����ߴ� �ڽ� �����ź��� ������ �� �ִ�.
        protected int currentSiblingIndex; 

        public List<Node> children { get; set; }
    }
}