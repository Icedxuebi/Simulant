using System.Runtime.InteropServices;

namespace Simulant.Game.FFCS.Client.Network
{
    [StructLayout(LayoutKind.Explicit, Size = 0xC50)]
    public unsafe partial struct NetworkModule
    {
        [FieldOffset(0xAA8)] public NetworkModulePacketReceiverCallback* PacketReceiverCallback;
    }
}