using System;

namespace Simulant.Game.FFCS.Client.Network
{
    public struct PacketDispatcher : IMemoryObject
    {
        public IntPtr Ptr { get; set; }

        // 收包函数 SpawnPlayer 分支调用的函数
        [SigPattern("48 89 5C 24 ?? 57 48 81 EC ?? ?? ?? ?? 48 8B DA 8B F9 0F B6 92")]
        public static IntPtr HandleSpawnPlayerPacketFuncPtr { get; set; }
        public static void HandleSpawnPlayerPacket(uint targetId, IntPtr packetPtr) // SpawnPlayerPacket*
        {
            HandleSpawnPlayerPacketFuncPtr.Call(targetId, packetPtr);
        }

        // 收包函数 SpawnNpc 分支调用的函数
        [SigPattern("48 89 5C 24 ?? 57 48 81 EC ?? ?? ?? ?? 48 8B DA 8B F9 E8 ?? ?? ?? ?? 3C ?? 75 ?? E8 ?? ?? ?? ?? 3C ?? 75 ?? 80 BB ?? ?? ?? ?? ?? 75 ?? 8B 05 ?? ?? ?? ?? 39 43 ?? 0F 85 ?? ?? ?? ?? 0F B6 53 ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 0F B6 53 ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 44 24 ?? C7 44 24 ?? ?? ?? ?? ?? BA ?? ?? ?? ?? 66 90 48 8D 80 ?? ?? ?? ?? ?? ?? ?? 0F 10 4B ?? 48 8D 9B ?? ?? ?? ?? 0F 11 40 ?? 0F 10 43 ?? 0F 11 48 ?? 0F 10 4B ?? 0F 11 40 ?? 0F 10 43 ?? 0F 11 48 ?? 0F 10 4B ?? 0F 11 40 ?? 0F 10 43 ?? 0F 11 48 ?? 0F 10 4B ?? 0F 11 40 ?? 0F 11 48 ?? 48 83 EA ?? 75 ?? ?? ?? ?? 4C 8D 44 24")]
        public static IntPtr HandleSpawnNpcPacketFuncPtr { get; set; }
        public static void HandleSpawnNpcPacket(uint targetId, IntPtr packetPtr) // SpawnNpcPacket*
        {
            HandleSpawnNpcPacketFuncPtr.Call(targetId, packetPtr);
        }

        // 收包函数 SpawnObject 分支调用的函数
        [SigPattern("40 53 57 48 83 EC ?? F6 42")]
        public static IntPtr HandleSpawnObjectPacketFuncPtr { get; set; }
        public static IntPtr HandleSpawnObjectPacket(uint targetId, IntPtr packetPtr) // SpawnObjectPacket*
        {
            return HandleSpawnObjectPacketFuncPtr.Call<IntPtr>(targetId, packetPtr);
        }

        // 收包函数 ActorMove 分支调用的函数，
        // 将 ActorMove 包的 payload 整体注册到全局实体移动管理器中，
        // 由其他消费函数统一插值处理移动
        // 仅对网络实体有效，也许以后找到方法将实体注册进网络实体列表后，可以换用这个控制假玩家的移动
        [SigPattern("48 89 5C 24 ?? 57 48 83 EC ?? 4C 63 05 ?? ?? ?? ??")]
        public static IntPtr HandleActorMovePacketFuncPtr { get; set; }
        public static IntPtr HandleActorMovePacket(uint objectId, IntPtr actorMovePayloadPtr) // ActorMovePayload*
        {
            return HandleActorMovePacketFuncPtr.Call<IntPtr>(objectId, actorMovePayloadPtr);
        }

        // 收包函数 ActorControl 等分支调用的函数
        // ActorControl 中 a5 后传参均固定为 0 (targetId 为 0xE0000000)
        // original "40 55 53 57 41 54 41 56 48 8D AC 24 ?? ?? ?? ?? B8 ?? ?? ?? ??"
        [SigPattern("44 8B 4F ?? 44 8B 47 ?? 89 44 24 ?? 8B 47 ?? 89 44 24 ?? E8 * * * *")]
        public static IntPtr HandleActorControlFuncPtr { get; set; }
        public static IntPtr HandleActorControl(uint objectId, ushort category, uint a1, uint a2, uint a3, uint a4, uint a5, uint a6, uint a7, uint a8, uint targetId, uint unk)
            => HandleActorControlFuncPtr.Call<IntPtr>(objectId, category, a1, a2, a3, a4, a5, a6, a7, a8, targetId, unk);

    }
}