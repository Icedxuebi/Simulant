using System.Runtime.InteropServices;

namespace Simulant.Game.FFCS.Client.Network
{
    [StructLayout(LayoutKind.Explicit, Size = 0x30)]
    public unsafe partial struct PacketDispatcher
    {
        [FieldOffset(0x10)] public NetworkModuleProxy* NetworkModuleProxy;
        [FieldOffset(0x18)] public uint GameSessionRandom;
        [FieldOffset(0x1C)] public uint LastPacketRandom;
        [FieldOffset(0x20)] public uint Key0;
        [FieldOffset(0x24)] public uint Key1;
        [FieldOffset(0x28)] public uint Key2;

        // [VirtualFunction(1)]
        // public partial void OnReceivePacket(uint targetId, nint packet);
    }
}