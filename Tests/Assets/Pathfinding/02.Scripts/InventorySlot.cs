using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pathfinding
{
    public class InventorySlot : MonoBehaviour
    {
        public int id
        {
            get => _id;
            set
            {
                if (value == _id)
                    return;

                _id = value;
                _icon.sprite = ItemSprites.instance[_id];
            }
        }
        private int _id;
        [SerializeField] private Image _icon;
    }
}