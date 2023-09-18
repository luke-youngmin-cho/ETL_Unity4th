using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance
{
    internal interface IHp
    {
        float HpValue { get; }
        float HpMax { get; }
        float HpMin { get; }
        void RecoverHp(float value);
        void DepleteHp(float value);
    }
}
