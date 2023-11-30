using RPG.Singleton;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RPG.DB
{
    public class GameDbContext : SingletonBase<GameDbContext>
    {
        public DbSet<InventoryModel> inventory;
        [Serializable]
        public class InventoryData
        {
            public InventoryData(List<InventoryModel> copy) => items = copy;

            public List<InventoryModel> items;
        }


        public void SaveChanges()
        {
            File.WriteAllText(inventory.path, JsonUtility.ToJson(new InventoryData(inventory.Entities)));
            Debug.Log($"[GameDbContext] : Saved all changes.");
        }

        protected override void Init()
        {
            base.Init();
            inventory = new DbSet<InventoryModel>();
            if (File.Exists(inventory.path) == false)
            {
                for (int i = 0; i < 32; i++)
                {
                    InventoryModel entity = inventory.Create();
                    entity.id = i;
                }
            }
            else
            {
                inventory.Entities 
                    = JsonUtility.FromJson<InventoryData>(File.ReadAllText(inventory.path)).items;
            }

            SaveChanges();
        }
    }
}