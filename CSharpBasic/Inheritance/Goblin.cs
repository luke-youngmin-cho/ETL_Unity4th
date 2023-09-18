using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance
{
    internal class Goblin : Enemy, IAttacker
    {
        public float AttackPower => throw new NotImplementedException();

        public void Attack(IHp target)
        {
            throw new NotImplementedException();
        }
    }
}
