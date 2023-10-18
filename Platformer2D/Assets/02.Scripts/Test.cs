using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class Test : UnityEngine.MonoBehaviour
    {
        GameObject _dummy;
        [SerializeField] private List<int> _list;
        private void Awake()
        {
            _dummy = new GameObject();
        }

        private void Reset()
        {
            _list = new List<int>()
            { 
                0, 1, 2, 3, 4
            };
        }

        private void OnEnable()
        {

        }

        private void Start()
        {

        }
    }

    public class Engine
    {
        public static Engine instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Engine();
                return _instance;
            }
        }
        private static Engine _instance;

        List<MonoBehaviour> monos = new List<MonoBehaviour>();

        public void RegisterMono(MonoBehaviour mono)
        {
            // MonoBehaviour 는 생성자오버로드를 개발자가 직접 구현해서 
            // 데이터 초기화부분을 작성할 수 가없다. 
            // 왜냐하면 MonoBehaviour 를 어떤 GameObject 에 Component 로서 붙여넣을땐
            // AddComponent 와 같은 함수를 통해서 내부적으로 정의된 생성자가 호출될 것이기 때문에.
            // 대신, MonoBehaviour 가 처음 생성되고 직후 Awake() 호출하게 해줌. 
            // -> 생성자에서 하던 데이터초기화를 Awake() 에서 진행하면 됨.
            mono.Awake();
            monos.Add(mono);
        }

        // Game Logic
        void Update()
        {
            foreach (var mono in monos)
            {
                // Update 하려는 MonoBehaviour 가 한번도 Start 된적 없으면
                // Start 한번만 호출해줌
                if (mono.hasStarted == false)
                {
                    mono.Start();
                    mono.hasStarted = true;
                }

                mono.Update();
            }
        }

        // Physics Logic
        void FixedUpdate()
        {
            foreach (var mono in monos)
            {
                mono.FixedUpdate();
            }
        }
    }

    public class GameObject
    {
        public bool enable
        {
            get => _enable;
            set
            {
                _enable = value;

                foreach (var mono in monos)
                {
                    if (mono.enable)
                        mono.enable = value;
                }
            }
        }
        private bool _enable;

        List<MonoBehaviour> monos = new List<MonoBehaviour>();

        public void AddComponent<T>()
            where T : MonoBehaviour
        {
            monos.Add(Activator.CreateInstance<T>());
        }
    }

    public class MonoBehaviour
    {
        public bool enable
        {
            get => _enable;
            set
            {
                _enable = value;

                // MonoBehaviour 가 활성/비활성 될때마다 호출할 함수들 : 
                if (value)
                    OnEnable();
                else
                    OnDisable();
            }
        }
        private bool _enable;
        public bool hasStarted;

        public MonoBehaviour()
        {
            //falfjhsadflaslfj
            Engine.instance.RegisterMono(this);
        }

        public void Awake() { }
        public void OnEnable() { }
        public void Start() { }
        public void Update() { }
        public void FixedUpdate() { }
        public void OnDisable() { }
    }

    
}