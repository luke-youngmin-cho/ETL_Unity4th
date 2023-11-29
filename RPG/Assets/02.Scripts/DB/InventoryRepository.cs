using System;
using System.Collections.Generic;

namespace RPG.DB
{
    public class InventoryRepository : IRepository<InventorySlot>
    {
        public InventorySlot Find(Predicate<InventorySlot> match)
        {
            throw new NotImplementedException();
        }

        public InventorySlot Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InventorySlot> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Insert(InventorySlot entity)
        {
            throw new NotImplementedException();
        }

        public void Update(InventorySlot entity)
        {
            throw new NotImplementedException();
        }
    }
}