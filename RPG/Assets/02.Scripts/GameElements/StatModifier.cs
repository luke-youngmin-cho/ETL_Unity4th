using RPG.DB.Local;
using System;

namespace RPG.GameElements
{
    public enum StatModType
    {
        None,
        FlatAdd,
        PercentAdd,
        PercentMul,
    }

    [Serializable]
    public class StatModifier
    {
        public StatID statID;
        public StatModType modType;
        public int value;
    }
}