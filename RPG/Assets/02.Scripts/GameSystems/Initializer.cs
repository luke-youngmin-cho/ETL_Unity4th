using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.GameSystems
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField] private List<GameObject> targets;
        private void Awake()
        {
            foreach (GameObject target in targets)
            {
                if (target.TryGetComponent(out IInitializable initializable))
                    initializable.Init();
            }
        }
    }
}