using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class Inventory : MonoBehaviour
    {
        private InventorySlot[] _slots;

        private void Start()
        {
            _slots = GetComponentsInChildren<InventorySlot>();

            _slots[0].id = 13;
            _slots[3].id = 21;
        }
    }
}