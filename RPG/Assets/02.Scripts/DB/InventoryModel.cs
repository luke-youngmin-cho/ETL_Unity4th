using System;

namespace RPG.DB
{
    [Serializable]
    public class InventoryModel : IDataModel
    {
        public int ID 
        { 
            get => id;
            set => id = value;
        }
        public int id;
        public int itemID;
        public int itemNum;
    }
}