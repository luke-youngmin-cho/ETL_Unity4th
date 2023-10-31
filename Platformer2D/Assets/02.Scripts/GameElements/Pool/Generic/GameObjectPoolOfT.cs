using UnityEngine;
using UnityEngine.Pool;

namespace Platformer.GameElements.Pool.Generic
{
    
    public class GameObjectPool<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        new public PoolTag tag;

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


        protected virtual T CreatePooledItem()
        {
            T item = Instantiate(_prefab);
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