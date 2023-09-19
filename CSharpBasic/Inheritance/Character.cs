using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance
{
    // abstract 키워드 :
    // 추상용도로 사용하는 것이므로 반드시 상속자가 해당 내용을 직접 구현해주어야 한다고 명시. 
    internal abstract class Character : IAttacker, IHp
    {
        // ? nullable : null 값 할당 가능하다고 명시하는 연산자
        public string? NickName { get; set; }

        //public string NickName
        //{
        //    get
        //    {
        //        return _nickName;
        //    }
        //    set
        //    {
        //        _nickName = value;
        //    }
        //}
        //private string _nickName;


        public float Exp
        {
            get
            {
                return _exp;
            }
            private set
            {
                if (value < 0)
                    value = 0;

                _exp = value;
            }
        }

        public float AttackPower
        {
            get
            {
                return _attackPower;
            }
        }

        public float HpValue => throw new NotImplementedException();

        public float HpMax => throw new NotImplementedException();

        public float HpMin => throw new NotImplementedException();

        private float _exp;
        private float _attackPower;

        //================================================================================
        //                                Public Methods
        //================================================================================

        // 추상함수 :
        // 숨쉬기라는 기능 자체는 필요한데 어떻게 동작할지는 잘 모르겠으니까 상속받은애들이 알아서 구현해
        public abstract void Breath();
             
        public float GetExp() 
        {
            return _exp; 
        }

        public void SetExp(float value)
        {
            if (value < 0)
                value = 0;

            _exp = value;
        }

        // virtual 가상 키워드
        // 가상함수 테이블에 이 함수 추가함
        public virtual void Jump()
        {
            Console.WriteLine("Jump!");
            // todo -> Add force 5 upward
        }

        public void Attack(IHp target)
        {
            target.DepleteHp(_attackPower);
        }

        public void RecoverHp(float value)
        {
            throw new NotImplementedException();
        }

        public void DepleteHp(float value)
        {
            throw new NotImplementedException();
        }


        //================================================================================
        //                                Private Methods
        //================================================================================
    }
}
