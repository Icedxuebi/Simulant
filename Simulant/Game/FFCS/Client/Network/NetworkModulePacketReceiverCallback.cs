using System.Runtime.InteropServices;
using System;

namespace Simulant.Game.FFCS.Client.Network
{
    // [Inherits<PacketReceiverCallbackInterface>, Inherits<PacketDispatcher>]
    public struct NetworkModulePacketReceiverCallback : IMemoryObject
    {
        public IntPtr Ptr { get; set; }
        public PacketDispatcher PacketDispatcher => Ptr.As<PacketDispatcher>(0x8);
    }
}
