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

        public event Action<float> onHpChanged; // float : 바뀐 이후 체력
        public event Action<float> onHpRecovered; // float : 회복된 양
        public event Action<float> onHpDepleted; // float : 깎인 양
        public event Action onHpMax;
        public event Action onHpMin;

        public void RecoverHp(object subject, float amount);
        public void DepleteHp(object subject, float amount);
    }
}