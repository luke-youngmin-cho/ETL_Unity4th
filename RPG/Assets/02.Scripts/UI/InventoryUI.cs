using RPG.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class InventoryUI : UIMonoBehaviour
    {
        private List<InventorySlot> _slots;
        [SerializeField] private InventorySlot _slotPrefab;
        private InventoryPresenter _presenter;
        private RectTransform _content;

        public override void Init()
        {
            base.Init();

            _content = GetComponentInChildren<GridLayoutGroup>()
                        .GetComponent<RectTransform>();
            _presenter = new InventoryPresenter();
            _slots = new List<InventorySlot>();
            foreach (var slotData in _presenter.inventorySource)
            {
                var slot = Instantiate(_slotPrefab, _content);
                slot.slotID = slotData.item.slotID;
                slot.Refresh(slotData.item.itemID, slotData.item.itemNum);
                _slots.Add(slot);
            }
            _presenter.inventorySource.OnItemChanged += (slotID, slotData) =>
            {
                var slot = _slots.Find(x => x.slotID == slotID);
                slot.Refresh(slotData.itemID, slotData.itemNum);
            };
        }
    }
}