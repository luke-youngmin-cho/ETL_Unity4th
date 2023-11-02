using Platformer.Stats;
using UnityEngine;

namespace Platformer.GameElements
{
    public class ProjectileWithDamage : Projectile
    {
        [HideInInspector] public float damage;

        protected override void OnHitTarget(RaycastHit2D hit)
        {
            base.OnHitTarget(hit);

            if (hit.collider.TryGetComponent(out IHp target))
            {
                target.DepleteHp(owner, damage);
            }
        }
    }
}