using Simulant.Game.FFCS.Client.Network;
using System;

namespace Simulant.Game.FFCS.Client.System.Framework
{
    public struct Framework : IMemoryObject
    {
        [SigPattern("48 8B 1D * * * * 8B 7C 24")]
        public static IntPtr InstancePtrPtr { get; set; }

        public static Framework Instance
            => InstancePtrPtr.ReadPtr().As<Framework>();

        public IntPtr Ptr { get; set; }

        public NetworkModuleProxy NetworkModuleProxy 
            => Ptr.ReadPtr(0x1678).As<NetworkModuleProxy>();
    }
}
