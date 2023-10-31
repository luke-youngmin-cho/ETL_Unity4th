using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Platformer.GameElements.Pool
{
    public class PoolManager
    {
        public static PoolManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PoolManager();
                }
                return _instance;
            }
        }
        private static PoolManager _instance;

        private Dictionary<PoolTag, IObjectPool<GameObject>> _pools = new Dictionary<PoolTag, IObjectPool<GameObject>>();

        /// <summary>
        /// 태그로 풀을 등록
        /// </summary>
        public void Register(PoolTag tag, IObjectPool<GameObject> pool)
        {
            _pools.Add(tag, pool);
        }

        /// <summary>
        /// 등록된 풀들중에서 태그로 특정 풀을 반환
        /// </summary>
        public IObjectPool<GameObject> GetPool(PoolTag tag)
        {
            return _pools[tag];
        }

        /// <summary>
        /// 특정 태그 풀에서 GameObject 하나 반환
        /// </summary>
        public GameObject Get(PoolTag tag)
        {
            return _pools[tag].Get();
        }

        /// <summary>
        /// 특정 태그 풀에서 GameObject 하나 가져온 후 GameObject 에서 T 타입 컴포넌트 찾아서 반환
        /// </summary>
        public T Get<T>(PoolTag tag)
        {
            return _pools[tag].Get().GetComponent<T>();
        }
    }
}