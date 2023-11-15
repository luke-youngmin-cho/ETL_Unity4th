using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Test.DataSerialization
{
    [Serializable]
    public struct Slot
    {
        public bool isEmpty => itemID <= 0 || itemNum <= 0;

        public int itemID;
        public int itemNum;
    }

    [DataContract]
    public class Inventory
    {
        public static Inventory instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Inventory();
                    _instance.Load();
                }
                return _instance;
            }
        }
        private static Inventory _instance;

        
        public Slot[] _slots;

        public Slot[] GetAll()
        {
            return _slots;
        }

        public void Load()
        {
            string path = Application.persistentDataPath + "/Inventory.json";
            if (System.IO.File.Exists(path) == false)
                CreateDefaultData();
            else
                _slots = JsonUtility.FromJson<Inventory>(System.IO.File.ReadAllText(path))._slots;
        }

        public void Save()
        {
            string json = JsonUtility.ToJson(this);
            string path = Application.persistentDataPath + "/Inventory.json";
            System.IO.File.WriteAllText(path, json);
        }

        private void CreateDefaultData()
        {
            _slots = new Slot[32];
            Save();
        }
    }
}