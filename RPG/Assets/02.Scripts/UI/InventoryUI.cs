using RPG.Collections.ObjectModel;
using RPG.DB.Local;
using RPG.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class InventoryUI : UIMonoBehaviour
    {
        private List<InventorySlot> _slots;
        [SerializeField] private InventorySlot _slotPrefab;
        [SerializeField] private Image _selectedSlotPreview;
        private int _selectedSlotID = -1;
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

        public override void InputAction()
        {
            base.InputAction();

            if (Input.GetMouseButtonDown(0))
            {
                // 선택된 슬롯이 없다면
                if (_selectedSlotID < 0)
                {
                    // 클릭위치에 슬롯이있으면 선택
                    if (CustomInputModule.main.TryGetHovered<GraphicRaycaster, InventorySlot>
                        (out InventorySlot slot))
                    {
                        if (_presenter.inventorySource[slot.slotID].itemID > 0)
                        {
                            Select(slot.slotID);
                            return;
                        }
                    }
                }
                // 선택된 슬롯이 있다면
                else
                {
                    if (CustomInputModule.main.TryGetHovered<GraphicRaycaster>
                        (out List<GameObject> hoveredList))
                    {
                        foreach (var hovered in hoveredList)
                        {
                            if (hovered.TryGetComponent(out InventorySlot slot))
                            {
                                if (slot != _slots[_selectedSlotID])
                                {
                                    if (_presenter.swapCommand.CanExecute(_selectedSlotID, slot.slotID))
                                    {
                                        _presenter.swapCommand.Execute(_selectedSlotID, slot.slotID);
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        NumberConfirmWindowUI confirmWindow = UIManager.instance.Get<NumberConfirmWindowUI>();
                        int slotID = _selectedSlotID;
                        confirmWindow.Show(message: "Enter number to throw.",
                                           max: _presenter.inventorySource[slotID].itemNum,
                                           onConfirm: () =>
                                           {
                                               InventorySlotData slotData = _presenter.inventorySource[slotID];
                                               int numToRemove = confirmWindow.numInput;
                                               if (slotData.itemNum >= numToRemove)
                                               {   
                                                   if (_presenter.removeCommand.CanExecute(slotID, slotData.itemID,numToRemove))
                                                   {
                                                       _presenter.removeCommand.Execute(slotID, slotData.itemID, numToRemove);
                                                       Instantiate(ItemInfoAssets.instance[slotData.itemID].pickableItemPrefab); // test
                                                   }
                                                   // todo -> Create world item gameobject
                                               }
                                           });
                    }
                }

                Deselect();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Deselect();
            }

            if (_selectedSlotID >= 0)
                _selectedSlotPreview.transform.position = Input.mousePosition;
        }

        private void Select(int slotID)
        {
            _selectedSlotID = slotID;
            _selectedSlotPreview.sprite = _slots[slotID].itemIcon;
            _selectedSlotPreview.gameObject.SetActive(true);
        }

        private void Deselect()
        {
            _selectedSlotID = -1;
            _selectedSlotPreview.sprite = null;
            _selectedSlotPreview.gameObject.SetActive(false);
        }
    }
}