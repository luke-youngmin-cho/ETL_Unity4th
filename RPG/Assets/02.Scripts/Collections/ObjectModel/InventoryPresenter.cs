using RPG.DB;
using UnityEngine;
using System;
using RPG.DB.Local;

namespace RPG.Collections.ObjectModel
{
    public struct InventorySlotData
    {
        public int slotID;
        public int itemID;
        public int itemNum;

        public InventorySlotData(int slotID, int itemID, int itemNum)
        {
            this.slotID = slotID;
            this.itemID = itemID;
            this.itemNum = itemNum;
        }
    }

    public class InventoryPresenter
    {
        public InventoryPresenter()
        {
            inventorySource = new InventorySource();
            swapCommand = new SwapCommand(this);
            removeCommand = new RemoveCommand(this);
            Debug.Log($"[InventoryPresenter] : Constructed");
        }

        public class InventorySource : ObservableCollection<InventorySlotData>
        {
            public InventorySource()
            {
                var entities = Repositories.instance.inventory.GetAll();
                foreach (var entity in entities)
                {
                    Items.Add(entity.ID,
                              new Pair<InventorySlotData>(entity.ID,
                                                          new InventorySlotData(entity.ID, entity.itemID, entity.itemNum)));
                }
            }
        }
        public InventorySource inventorySource;

        public class SwapCommand
        {
            public SwapCommand(InventoryPresenter presenter)
            {
                _presenter = presenter;
                _inventorySource = _presenter.inventorySource;
            }

            InventoryPresenter _presenter;
            InventorySource _inventorySource;

            public bool CanExecute(int slotID1, int slotID2)
            {
                return slotID1 != slotID2 &&
                       _inventorySource.Contains(slotID1) &&
                       _inventorySource.Contains(slotID2);
            }

            public void Execute(int slotID1, int slotID2)
            {
                InventoryModel slot1Data = Repositories.instance.inventory.Get(slotID1);
                InventoryModel slot2Data = Repositories.instance.inventory.Get(slotID2);

                if (slot1Data == null || slot2Data == null)
                    throw new Exception($"[InventoryPresenter] : Failed to swap {slotID1}, {slotID2}.");

                InventorySlotData expectedSlot1;
                InventorySlotData expectedSlot2;

                if (slot1Data.itemID == slot2Data.itemID)
                {
                    int max = ItemInfoAssets.instance[slot1Data.itemID].maxNum;
                    int capacity = max - slot2Data.itemNum;
                    int remains = slot1Data.itemNum - capacity;

                    expectedSlot1 = new InventorySlotData()
                    {
                        slotID = slotID1,
                        itemID = remains > 0 ? slot2Data.itemID : 0,
                        itemNum = remains > 0 ? remains : 0,
                    };
                    expectedSlot2 = new InventorySlotData()
                    {
                        slotID = slotID2,
                        itemID = slot1Data.itemID,
                        itemNum = remains > 0 ? max : slot1Data.itemNum + slot2Data.itemNum,
                    };
                }
                else
                {
                    expectedSlot1 = new InventorySlotData()
                    {
                        slotID = slotID1,
                        itemID = slot2Data.itemID,
                        itemNum = slot2Data.itemNum,
                    };
                    expectedSlot2 = new InventorySlotData()
                    {
                        slotID = slotID2,
                        itemID = slot1Data.itemID,
                        itemNum = slot1Data.itemNum,
                    };
                }

                Repositories.instance.inventory.Update(new InventoryModel()
                {
                    id = expectedSlot1.slotID,
                    itemID = expectedSlot1.itemID,
                    itemNum = expectedSlot1.itemNum,
                });
                Repositories.instance.inventory.Update(new InventoryModel()
                {
                    id = expectedSlot2.slotID,
                    itemID = expectedSlot2.itemID,
                    itemNum = expectedSlot2.itemNum,
                });

                Repositories.instance.SaveChanges();

                _inventorySource.Change(slotID1, expectedSlot1);
                _inventorySource.Change(slotID2, expectedSlot2);
            }
        }
        public SwapCommand swapCommand;

        public class RemoveCommand
        {
            public RemoveCommand(InventoryPresenter presenter)
            {
                _presenter = presenter;
                _inventorySource = _presenter.inventorySource;
            }

            private InventoryPresenter _presenter;
            private InventorySource _inventorySource;

            public bool CanExecute(int slotID, int itemID, int itemNum)
            {
                if (_inventorySource.Contains(slotID) == false)
                    return false;

                if (_inventorySource[slotID].itemID != itemID)
                    return false;

                if (_inventorySource[slotID].itemNum < itemNum)
                    return false;

                return true;
            }

            public void Execute(int slotID, int itemID, int itemNum)
            {
                InventoryModel slotData = Repositories.instance.inventory.Get(slotID);

                if (slotData.itemID != _inventorySource[slotID].itemID ||
                    slotData.itemNum != _inventorySource[slotID].itemNum)
                    throw new Exception($"[InventoryPresenter.RemoveCommand] : Data is not matched");

                int remains = slotData.itemNum - itemNum;
                if (remains < 0)
                    throw new Exception($"[InventoryPresenter.RemoveCommand] : Something wrong...");

                InventorySlotData expectedSlot = new InventorySlotData(slotID,
                                                                       remains > 0 ? itemID : 0,
                                                                       remains);

                Repositories.instance.inventory.Update(new InventoryModel()
                {
                    id = expectedSlot.slotID,
                    itemID = expectedSlot.itemID,
                    itemNum = expectedSlot.itemNum,
                });
                Repositories.instance.SaveChanges();

                _inventorySource[slotID] = expectedSlot;
            }
        }
        public RemoveCommand removeCommand;
    }
}