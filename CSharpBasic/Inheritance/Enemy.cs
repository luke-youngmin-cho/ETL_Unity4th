using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance
{
    internal abstract class Enemy : IHp
    {
        public float HpValue => throw new NotImplementedException();

        public float HpMax => throw new NotImplementedException();

        public float HpMin => throw new NotImplementedException();

        public void DepleteHp(float value)
        {
            throw new NotImplementedException();
        }

        public void RecoverHp(float value)
        {
            throw new NotImplementedException();
        }
    }
}
