using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Animations
{

    public class CharacterAnimationEvents : MonoBehaviour
    {
        public Action onHit;

        private void Hit()
        {
            onHit?.Invoke();
        }
    }
}