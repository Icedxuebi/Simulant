using Simulant.ACT;
using System;
using System.Runtime.InteropServices;

namespace Simulant.Game.Packets
{
    public class ActorMove
    {
        // 收包函数 ActorMove 分支调用的函数，
        // 将 ActorMove 包的 payload 整体注册到全局实体移动管理器中，
        // 由其他消费函数统一插值处理移动

        // 仅对网络实体有效，也许以后找到方法将实体注册进网络实体列表后，可以换用这个控制假玩家的移动
        [SigPattern("48 89 5C 24 ?? 57 48 83 EC ?? 4C 63 05 ?? ?? ?? ??")]
        public static IntPtr QueueActorMovePacketFuncPtr { get; set; }
        public static IntPtr QueueActorMovePacket(uint objectId, IntPtr actorMovePayloadPtr) // ActorMovePayload*
        {
            return QueueActorMovePacketFuncPtr.Call<IntPtr>(objectId, actorMovePayloadPtr);
        }

        public static IntPtr QueueActorMove(uint objectId, float x, float y, float z, float heading, byte speedParam)
        {
            ActorMovePayload payload = new ActorMovePayload
            {
                RawX = Helper.EncodeUShortCoord(x),
                RawY = Helper.EncodeUShortCoord(z), // y ↔ z
                RawZ = Helper.EncodeUShortCoord(y),
                RawHeading = Helper.EncodeUShortHeading(heading),
                SpeedParam = speedParam,
                Unk05 = 0,
                Unk0C = 0
            };

            var actorMovePayloadPtr = NamazuInterop.Plugin.Memory.AllocateMemory(Marshal.SizeOf<ActorMovePayload>());
            try
            {
                actorMovePayloadPtr.Write(payload);
                return QueueActorMovePacket(objectId, actorMovePayloadPtr);
            }
            finally
            {
                NamazuInterop.Plugin.Memory.FreeMemory(actorMovePayloadPtr);
            }
        }

        // ACT 日志：270 10E:40004574:-1.2434:0000:003C:100.3891:99.2904:0.0457
        [StructLayout(LayoutKind.Explicit, Size = 0x10, Pack = 1)]
        private struct ActorMovePayload
        {
            [FieldOffset(0x00)]
            public ushort RawHeading;

            [FieldOffset(0x02)]
            public ushort Flags;

            [FieldOffset(0x04)]
            public byte SpeedParam; // 仅用到了低 8 位，如 0x14 = 20, 0x3C = 60

            [FieldOffset(0x05)]
            public byte Unk05;

            [FieldOffset(0x06)]
            public ushort RawX;

            [FieldOffset(0x08)]
            public ushort RawY;

            [FieldOffset(0x0A)]
            public ushort RawZ;

            [FieldOffset(0x0C)]
            public uint Unk0C;
        }
    }
}
