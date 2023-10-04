using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates
{
    internal class PlayerUI
    {
        private string _hpValue;

        public void Refresh(float hpValue)
        {
            _hpValue = hpValue.ToString();
        }
    }
}
