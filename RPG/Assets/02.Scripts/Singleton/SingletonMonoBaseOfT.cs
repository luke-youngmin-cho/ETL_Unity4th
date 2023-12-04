using System;
using System.Reflection;
using UnityEngine;

namespace RPG.Singleton
{
    public class SingletonMonoBase<T> : MonoBehaviour
        where T : SingletonMonoBase<T>
    {
        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    _instance.Init();
                }
                return _instance;
            }
        }
        private static T _instance;

        protected virtual void Init()
        {
        }
    }
}