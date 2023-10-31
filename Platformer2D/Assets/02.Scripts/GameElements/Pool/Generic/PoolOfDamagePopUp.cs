using Platformer.Effects;
using UnityEngine.Pool;
using UnityEngine;

namespace Platformer.GameElements.Pool.Generic
{
    public class PoolOfDamagePopUp : GameObjectPool<DamagePopUp>
    {
        public class PooledItem : MonoBehaviour
        {
            public IObjectPool<DamagePopUp> pool;
            private DamagePopUp _item;

            private void Awake()
            {
                _item = GetComponent<DamagePopUp>();
            }

            private void OnDisable()
            {
                ReturnToPool();
            }

            public void ReturnToPool()
            {
                pool.Release(_item);
                Debug.Log($"Returned to pool");
            }
        }

        private void Awake()
        {
            PoolManagerOfDamagePopUP.instance.Register(tag, pool);
        }
    }
}