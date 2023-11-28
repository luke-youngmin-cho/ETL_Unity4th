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
                    // Reflection(��Ÿ���� ��Ÿ�����������ϴ� ����� �ִ�..) ���� ������ ���� �޾ƿͼ� ������ȣ��
                    //Type type = typeof(T);
                    //ConstructorInfo constructorInfo = type.GetConstructor(new Type[] { });
                    //_instance = (T)constructorInfo.Invoke(null);

                    // �ν��Ͻ��� �����ϴµ� �ʿ��� ��ɵ��� �����ϴ� Activator
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