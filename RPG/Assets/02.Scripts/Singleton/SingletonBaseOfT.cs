using System;
using System.Reflection;

namespace RPG.Singleton
{
    public class SingletonBase<T>
        where T : SingletonBase<T>
    {
        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    // Reflection(런타임중 메타데이터접근하는 기능이 있는..) 으로 생성자 정보 받아와서 생성자호출
                    //Type type = typeof(T);
                    //ConstructorInfo constructorInfo = type.GetConstructor(new Type[] { });
                    //_instance = (T)constructorInfo.Invoke(null);

                    // 인스턴스를 생성하는데 필요한 기능들을 제공하는 Activator
                    _instance = Activator.CreateInstance<T>();
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