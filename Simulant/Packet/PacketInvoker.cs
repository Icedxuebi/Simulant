using Simulant.ACT;
using Simulant.Game.FFCS.Client.Game.Network;
using Simulant.Game.FFCS.Client.Network;
using Simulant.Packet;
using System;
using System.Runtime.InteropServices;

namespace Simulant.Game
{
    public static class PacketInvoker
    {

        public static IntPtr ActorControl(uint objectId, ushort category, uint a1 = 0, uint a2 = 0, uint a3 = 0, uint a4 = 0)
        {
            uint a5 = 0;
            uint a6 = 0;
            uint a7 = 0;
            uint a8 = 0;
            uint targetId = 0xE0000000u;
            uint unk = 0;

            return PacketDispatcher.HandleActorControl(objectId, category, a1, a2, a3, a4, a5, a6, a7, a8, targetId, unk);
        }

        public static IntPtr ActorMove(uint objectId, float x, float y, float z, float heading, byte speedParam)
        {
            var payload = new ActorMovePacket
            {
                RawX = PacketCodec.EncodeUShortCoord(x),
                RawY = PacketCodec.EncodeUShortCoord(z), // y ↔ z
                RawZ = PacketCodec.EncodeUShortCoord(y),
                RawHeading = PacketCodec.EncodeUShortHeading(heading),
                SpeedParam = speedParam
            };

            var payloadPtr = NamazuInterop.Plugin.Memory.AllocateMemory(Marshal.SizeOf<ActorMovePacket>());
            try
            {
                payloadPtr.Write(payload);
                return PacketDispatcher.HandleActorMovePacket(objectId, payloadPtr);
            }
            finally
            {
                NamazuInterop.Plugin.Memory.FreeMemory(payloadPtr);
            }
        }

        public static IntPtr ObjectSpawn(SpawnObjectData data)
        {
            var payload = new SpawnObjectPacket
            {
                ObjectIndex = data.ObjectIndex,
                ObjectKind = (byte)data.ObjectKind,
                TargetableStatus = data.TargetableStatus,
                Visibility = data.Visibility,

                BaseId = data.BaseId,
                EntityId = data.Id,
                LayoutId = data.LayoutId,
                OwnerId = data.OwnerId,
                GimmickId = data.GimmickId,

                Radius = data.Radius,
                Rotation = PacketCodec.EncodeUShortHeading(data.Heading),

                FateId = data.FateId,
                EventState = data.EventState,

                PositionX = data.X,
                PositionY = data.Z, // y ↔ z
                PositionZ = data.Y
            };

            var payloadPtr = NamazuInterop.Plugin.Memory.AllocateMemory(Marshal.SizeOf<SpawnObjectPacket>());
            try
            {
                payloadPtr.Write(payload);
                return PacketDispatcher.HandleSpawnObjectPacket(data.Id, payloadPtr);
            }
            finally
            {
                NamazuInterop.Plugin.Memory.FreeMemory(payloadPtr);
            }
        }

    }
}
