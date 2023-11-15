using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Test.DataSerialization
{
    public class InventoryUI : MonoBehaviour
    {
        private RectTransform _content;

        private void Start()
        {
            _content = GetComponentInChildren<GridLayoutGroup>().GetComponent<RectTransform>();
            Slot[] data = Inventory.instance.GetAll();
            for (int i = 0; i < data.Length; i++)
            {
                new GameObject("Slot")
                    .AddComponent<Image>()
                    .transform
                    .SetParent(_content);
            }

            Quests.Load();
        }
    }
}