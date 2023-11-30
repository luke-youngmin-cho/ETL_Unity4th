using UnityEngine;

namespace RPG.DB.Local
{
    [CreateAssetMenu(fileName = "new ItemInfo", menuName = "RPG/ScriptableObjects/ItemInfo")]
    public class ItemInfo : ScriptableObject
    {
        [field: SerializeField] public int id { get; private set; }
        [field: SerializeField] public string description { get; private set; }
        [field: SerializeField] public Sprite icon { get; private set; }
        [field: SerializeField] public int maxNum { get; private set; }
    }
}