using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;

namespace CopycatOverCooked.GamePlay
{
    public interface IUsable
    {
        void Use(NetworkObject user);
    }
}
