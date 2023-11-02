using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Animations
{

    public class CharacterAnimationEvents : MonoBehaviour
    {
        public Action onHit;
        public Action onLaunchProjectile;


        private void Hit()
        {
            onHit?.Invoke();
        }

        private void LaunchProjectile()
        {
            onLaunchProjectile?.Invoke();
        }
    }
}