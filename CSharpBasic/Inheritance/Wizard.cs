using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance
{
    internal class Wizard : Character
    {
        public override void Breath()
        {
            throw new NotImplementedException();
        }

        public override void Jump()
        { 
            // base 키워드
            // 기반타입 참조 키워드
            base.Jump();
            // todo -> add effects
        }

        public void FireBall()
        {
            Console.WriteLine("Fire ball !");
        }
    }
}
