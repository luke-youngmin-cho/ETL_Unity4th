using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Platformer.GameElements.Pool
{
    public class ParticleSystemPoolManager
    {
        public static ParticleSystemPoolManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ParticleSystemPoolManager();
                }
                return _instance;
            }
        }
        private static ParticleSystemPoolManager _instance;

        private Dictionary<PoolTag, IObjectPool<ParticleSystem>> _pools = new Dictionary<PoolTag, IObjectPool<ParticleSystem>>();

        /// <summary>
        /// 태그로 풀을 등록
        /// </summary>
        public void Register(PoolTag tag, IObjectPool<ParticleSystem> pool)
        {
            _pools.Add(tag, pool);
        }

        /// <summary>
        /// 등록된 풀들중에서 태그로 특정 풀을 반환
        /// </summary>
        public IObjectPool<ParticleSystem> GetPool(PoolTag tag)
        {
            return _pools[tag];
        }

        /// <summary>
        /// 특정 태그 풀에서 GameObject 하나 반환
        /// </summary>
        public ParticleSystem Get(PoolTag tag)
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