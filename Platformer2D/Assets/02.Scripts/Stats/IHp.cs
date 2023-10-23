using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Stats
{
    public interface IHp
    {
        public bool invincible { get; set; }
        public float hpValue { get; set; }
        public float hpMax { get; }
        public float hpMin { get; }

        public event Action<float> onHpChanged;
        public event Action<float> onHpRecovered;
        public event Action<float> onHpDepleted;
        public event Action onHpMax;
        public event Action onHpMin;

        public void RecoverHp(object subject, float amount);
        public void DepleteHp(object subject, float amount);
    }
}