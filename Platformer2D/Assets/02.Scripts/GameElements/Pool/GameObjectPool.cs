using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Platformer.GameElements.Pool
{
    public class GameObjectPool : MonoBehaviour
    {
        new public PoolTag tag;

        public class PooledItem : MonoBehaviour
        {
            public IObjectPool<GameObject> pool;
            public Action onReturnToPool;

            private void OnDisable()
            {
                ReturnToPool();
            }

            public void ReturnToPool()
            {
                pool.Release(gameObject);
                Debug.Log($"Returned to pool");
                onReturnToPool?.Invoke();
            }
        }

        public enum PoolType
        {
            Stack,
            LinkedList
        }
        [SerializeField] private PoolType _collectionType;
        [SerializeField] private bool _collectionCheck;

        public IObjectPool<GameObject> pool
        {
            get
            {
                if (_pool == null)
                {
                    if (_collectionType == PoolType.Stack)
                        _pool = new ObjectPool<GameObject>(CreatePooledItem,
                                                            OnGetFromPool,
                                                            OnReturnToPool,
                                                            OnDestroyPooledItem,
                                                            _collectionCheck,
                                                            _defaultCapacity,
                                                            _sizeMax);
                    else
                        _pool = new LinkedPool<GameObject>(CreatePooledItem,
                                                            OnGetFromPool,
                                                            OnReturnToPool,
                                                            OnDestroyPooledItem,
                                                            _collectionCheck,
                                                            _sizeMax);
                }
                return _pool;
            }
        }
        private IObjectPool<GameObject> _pool;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _defaultCapacity;
        [SerializeField] private int _sizeMax;

        private void Awake()
        {
            PoolManager.instance.Register(tag, pool);
        }

        private GameObject CreatePooledItem()
        {
            GameObject item = Instantiate(_prefab);
            item.AddComponent<PooledItem>().pool = pool;
            return item;
        }

        private void OnGetFromPool(GameObject gameObject)
        {
            gameObject.SetActive(true);
        }

        private void OnReturnToPool(GameObject gameObject)
        {
            gameObject.SetActive(false);
        }

        private void OnDestroyPooledItem(GameObject gameObject)
        {
            Destroy(gameObject);
        }
    }
}