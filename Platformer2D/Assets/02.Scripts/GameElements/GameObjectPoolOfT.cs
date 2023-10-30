using UnityEngine;
using UnityEngine.Pool;

namespace Platformer.GameElements
{
    public enum PoolTag
    {
        None,
        DamagePopUp_Player,
        DamagePopUP_Enemy,
        Enemy_Slug,
    }

    public class GameObjectPool<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        new public PoolTag tag;

        public class PooledItem : MonoBehaviour
        {
            public IObjectPool<T> pool;
            private T _item;

            private void Awake()
            {
                _item = GetComponent<T>();
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

        public enum PoolType
        {
            Stack,
            LinkedList
        }
        [SerializeField] private PoolType _collectionType;
        [SerializeField] private bool _collectionCheck;

        public IObjectPool<T> pool
        {
            get
            {
                if (_pool == null)
                {
                    if (_collectionType == PoolType.Stack)
                        _pool = new ObjectPool<T>(CreatePooledItem,
                                                  OnGetFromPool,
                                                  OnReturnToPool,
                                                  OnDestroyPooledItem,
                                                  _collectionCheck,
                                                  _count,
                                                  _countMax);
                    else
                        _pool = new LinkedPool<T>(CreatePooledItem,
                                                  OnGetFromPool,
                                                  OnReturnToPool,
                                                  OnDestroyPooledItem,
                                                  _collectionCheck,
                                                  _countMax);
                }
                return _pool;
            }
        }
        private IObjectPool<T> _pool;
        [SerializeField] private T _prefab;
        [SerializeField] private int _count;
        [SerializeField] private int _countMax;

        private void Awake()
        {
            PoolManager<T>.instance.Register(tag, pool);
        }


        protected virtual T CreatePooledItem()
        {
            T item = Instantiate(_prefab);
            item.gameObject.AddComponent<PooledItem>().pool = pool;
            return item;
        }

        protected virtual void OnGetFromPool(T item)
        {
            item.gameObject.SetActive(true);
        }

        protected virtual void OnReturnToPool(T item)
        {
            item.gameObject.SetActive(false);
        }

        protected virtual void OnDestroyPooledItem(T item)
        {
            Destroy(item.gameObject);
        }
    }
}