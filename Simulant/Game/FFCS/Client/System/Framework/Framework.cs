using Simulant.Game.FFCS.Client.Network;
using System.Runtime.InteropServices;

namespace Simulant.Game.FFCS.Client.System.Framework
{
    [StructLayout(LayoutKind.Explicit, Size = 0x35D8)]
    public unsafe partial struct Framework
    {
        [SigPattern("48 8B 1D * * * * 8B 7C 24")]
        public static Framework* Instance { get; set; }

        [FieldOffset(0x1678)] public NetworkModuleProxy* NetworkModuleProxy;
    }
}
