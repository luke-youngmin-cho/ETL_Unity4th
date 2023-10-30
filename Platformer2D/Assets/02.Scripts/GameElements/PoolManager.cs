using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine;

namespace Platformer.GameElements
{
    public class PoolManager<T>
        where T : MonoBehaviour
    {
        public static PoolManager<T> instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PoolManager<T>();
                return _instance;
            }
        }
        private static PoolManager<T> _instance;
        private Dictionary<PoolTag, IObjectPool<T>> _pools = new Dictionary<PoolTag, IObjectPool<T>>();

        public IObjectPool<K> GetPool<K>(PoolTag tag)
            where K : MonoBehaviour
        {
            return (IObjectPool<K>)_pools[tag];
        }

        public K Get<K>(PoolTag tag)
            where K : MonoBehaviour
        {
            return GetPool<K>(tag).Get();
        }

        public void Register(PoolTag tag, IObjectPool<T> pool)
        {
            _pools.Add(tag, pool);
        }
    }
}