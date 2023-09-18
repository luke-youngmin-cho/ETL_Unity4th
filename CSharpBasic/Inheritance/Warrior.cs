using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance
{
    internal class Warrior : Character
    {
        public float anger;

        public void Slash()
        {
            Console.WriteLine("Slash!");
        }
    }
}
