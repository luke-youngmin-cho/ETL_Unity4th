using UnityEngine;
using UnityEngine.Pool;

namespace Platformer.Effects
{
    public class PoolOfDamagePopUp : MonoBehaviour
    {
        public class PooledItem : MonoBehaviour
        {
            public IObjectPool<DamagePopUp> pool;
            private DamagePopUp _damagePopUp;

            private void Awake()
            {
                _damagePopUp = GetComponent<DamagePopUp>();
            }

            private void OnDisable()
            {
                ReturnToPool();
            }

            public void ReturnToPool()
            {
                pool.Release(_damagePopUp);
                Debug.Log($"Returned to pool");
            }
        }

        public enum PoolType
        {
            Stack,
            LinkedList
        }
        [SerializeField] private PoolType _collectionType;
        [SerializeField] private bool _collectionCheck;

        public IObjectPool<DamagePopUp> pool
        {
            get
            {
                if (_pool == null)
                {
                    if (_collectionType == PoolType.Stack)
                        _pool = new ObjectPool<DamagePopUp>(CreatePooledItem,
                                                            OnGetFromPool,
                                                            OnReturnToPool,
                                                            OnDestroyPooledItem,
                                                            _collectionCheck,
                                                            _count,
                                                            _countMax);
                    else
                        _pool = new LinkedPool<DamagePopUp>(CreatePooledItem,
                                                            OnGetFromPool,
                                                            OnReturnToPool,
                                                            OnDestroyPooledItem,
                                                            _collectionCheck,
                                                            _countMax);
                }
                return _pool;
            }
        }
        private IObjectPool<DamagePopUp> _pool;
        [SerializeField] private DamagePopUp _prefab;
        [SerializeField] private int _count;
        [SerializeField] private int _countMax;

        private DamagePopUp CreatePooledItem()
        {
            DamagePopUp item = Instantiate(_prefab);
            item.gameObject.AddComponent<PooledItem>().pool = pool;
            return item;
        }

        private void OnGetFromPool(DamagePopUp damagePopUp)
        {
            damagePopUp.gameObject.SetActive(true);
        }

        private void OnReturnToPool(DamagePopUp damagePopUp)
        {
            damagePopUp.gameObject.SetActive(false);
        }

        private void OnDestroyPooledItem(DamagePopUp damagePopUp)
        {
            Destroy(damagePopUp.gameObject);
        }
    }
}