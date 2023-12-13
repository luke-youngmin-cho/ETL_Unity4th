using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.DB.Local
{
    public abstract class ID : ScriptableObject
    {
        [field: SerializeField] public int value { get; private set; }
    }
}