using RPG.DB.Local;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class InventorySlot : MonoBehaviour
    {
        public Sprite itemIcon => _itemIcon.sprite;
        public int slotID;
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TMP_Text _itemNum;

        public void Refresh(int itemID, int itemNum)
        {
            if (itemID > 0 && itemNum > 0)
            {
                _itemIcon.sprite = ItemInfoAssets.instance[itemID].icon;
                _itemNum.text = itemNum == 1 ? string.Empty : itemNum.ToString();
            }
            else
            {
                _itemIcon.sprite = null;
                _itemNum.text = string.Empty;
            }
        }
    }
}