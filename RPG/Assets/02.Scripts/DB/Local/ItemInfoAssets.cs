using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.DB.Local
{
    public class ItemInfoAssets : MonoBehaviour
    {
        public static ItemInfoAssets instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Instantiate(Resources.Load<ItemInfoAssets>("ItemInfoAssets"));
                    _instance.Init();
                }
                return _instance;
            }
        }
        private static ItemInfoAssets _instance;

        public ItemInfo this[int itemID] => _dictionary[itemID];
        private Dictionary<int, ItemInfo> _dictionary;
        [SerializeField] private List<ItemInfo> _list;

        private void Init()
        {
            _dictionary = new Dictionary<int, ItemInfo>();
            foreach (var item in _list)
            {
                _dictionary.Add(item.id, item);
            }
        }
    }
}