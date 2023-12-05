using System.Collections.Generic;

namespace RPG.AISystems.BT
{
    public interface IParentOfChildren
    {
        List<Node> children { get; set; }
    }
}