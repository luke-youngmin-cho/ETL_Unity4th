using RPG.DB;
using UnityEngine;

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
    }
}