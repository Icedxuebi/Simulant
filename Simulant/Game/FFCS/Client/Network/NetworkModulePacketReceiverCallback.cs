using System.Runtime.InteropServices;

namespace Simulant.Game.FFCS.Client.Network
{
    // [Inherits<PacketReceiverCallbackInterface>, Inherits<PacketDispatcher>]
    [StructLayout(LayoutKind.Explicit, Size = 0x38)]
    public partial struct NetworkModulePacketReceiverCallback
    {
        // [FieldOffset(0x0)] public PacketReceiverCallbackInterface* PacketReceiverCallbackInterface;
        [FieldOffset(0x8)] public PacketDispatcher PacketDispatcher;
    }
}
