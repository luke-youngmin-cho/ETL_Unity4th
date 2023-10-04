using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates
{
    internal class Player
    {

        public float hp
        {
            get
            {
                return _hp;
            }
            set
            {
                if (_hp == value) 
                    return;

                _hp = value;
                // 직접호출. 구독된 모든 함수 차례대로 호출 후 반환값은 마지막 호출한 함수 반환값
                onHpChanged(value);
                // 간접호출. 의미없는 함수호출스택을 통해서 대리자에 등록된 함수들을 호출하기때문에 
                // 참조하던 원본 데이터가 있는 변수가 수정되더라도 상관없이 의도한 결과를 낼 수 있음
                onHpChanged.Invoke(value);
                // Null Check 연산자 : 해당변수가 Null 이면 그뒤 멤버접근 하지않고 Null 반환
                onHpChanged?.Invoke(value);

                //_ui.Refresh(value);
                //_ui2.Refresh(value);
            }
        }

        private float _hp;

        // 대리자 타입 정의
        public delegate void OnHpChangedHandler(float value);
        public delegate void OnHpChangedHandlerForInt(int value);
        public delegate void OnHpChangedHandlerForDouble(double value);
        // event 한정자 
        // 해당 대리자를 외부에서는 구독하기 (+=), 구독취소하기 (-=) 기능으로밖에 사용하지 못하도록 제한함.
        public event OnHpChangedHandler onHpChanged;
        //public event Action<float> onHpChanged;

        // Action 대리자 : void 를 반환하고 파라미터를 0 ~ N 개 받을 수 있는 형태의 함수를 구독할 때 사용
        public event Action action1;
        public event Action<float> action2;
        public event Action<int> action3;
        public event Action<double> action4;
        public event Action<int, string, float> action5;

        // Func 대리자 : T 를 반환하고 파라미터를 0~N 개 받을 수 있는 형태의 함수를 구독할 때 사용
        // Generic 타입 명시는 가장 오른쪽에 반환타입, 앞에서부터는 파라미터타입
        public event Func<int, bool> func1;
        public event Func<string, string, bool> func2;

        // Predicate 대리자 : 파라미터 1개 받고, bool 타입을 반환하는 함수 구독시 사용
        public event Predicate<int> match1;

        private PlayerUI _ui;
        private PlayerUI2 _ui2;
        public Player(PlayerUI ui, PlayerUI2 ui2)
        {
            _ui = ui;
            _ui2 = ui2;
            func1 += IsBiggerThan4;
            func1.Invoke(5);
        }

        public Player() { }

        bool IsBiggerThan4(int value)
        {
            return value > 4;
        }


    }
}
