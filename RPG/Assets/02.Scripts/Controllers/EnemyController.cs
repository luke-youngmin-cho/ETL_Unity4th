using RPG.AISystems.BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tree = RPG.AISystems.BT.Tree;

namespace RPG.Controllers
{
    public class EnemyController : CharacterController
    {
        private Tree _tree;

        private void Start()
        {
            _tree = gameObject.AddComponent<Tree>();
            _tree.StartBuild()
                .Selector()
                    .Sequence()
                        .Execution(() => Result.Success)
                    .ExitCurrentComposite()
                    .Sequence()
                        .Execution(() => Result.Success);


            //_tree.root.child = new Selector(_tree);
            //((Selector)_tree.root.child).children.Add(new Sequence(_tree));
            //((Sequence)((Selector)_tree.root.child).children[0]).children.Add(new Execution(_tree, () => Result.Success));
        }
    }
}