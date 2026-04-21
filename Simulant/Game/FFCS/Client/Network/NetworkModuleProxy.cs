using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Simulant.Game.FFCS.Client.Network
{
    [StructLayout(LayoutKind.Explicit, Size = 0x20)]
    public unsafe partial struct NetworkModuleProxy
    {
        [FieldOffset(0x08)] public NetworkModule* NetworkModule;
    }
}

