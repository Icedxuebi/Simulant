using System;

namespace Simulant.Game.FFCS.Client.Network
{
    public struct PacketDispatcher : IMemoryObject
    {
        public IntPtr Ptr { get; set; }
    }
}