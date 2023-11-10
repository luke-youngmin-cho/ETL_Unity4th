using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class ItemSprites : MonoBehaviour
    {
        public static ItemSprites instance;

        public Sprite this[int id] => sprites[id];

        public List<Sprite> sprites;

        private void Awake()
        {
            instance = this;
        }
    }
}