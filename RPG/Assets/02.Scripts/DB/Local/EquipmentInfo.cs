using RPG.GameElements;
using UnityEngine;

namespace RPG.DB.Local
{
    [CreateAssetMenu(fileName = "new EquipmentInfo", menuName = "RPG/ScriptableObjects/EquipmentInfo")]
    public class EquipmentInfo : ItemInfo
    {
        [field: SerializeField] public StatModifier[] statModifiers { get; private set; }
    }
}