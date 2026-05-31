using Simulant.Game;
using Simulant.Game.FFCS.Client.Game.Character;
using Simulant.Game.FFCS.Client.Game.Network;
using Simulant.Game.FFCS.Client.Game.Object;
using Simulant.Game.FFCS.Client.Network;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Numerics;

namespace Simulant.Core.Entity
{
    public sealed class EntitySpawner
    {
        private readonly PluginHost _host;

        public EntitySpawner(PluginHost host)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
        }

        private readonly ConcurrentDictionary<IntPtr, ushort> _localEntityIndexes =
            new ConcurrentDictionary<IntPtr, ushort>();

        public Character CreateBattleCharacter()
        {
            var createdIndex = ClientObjectManager.Instance.CreateBattleCharacter();
            if (createdIndex > ushort.MaxValue)
                throw new InvalidOperationException($"生成实体失败：返回的实体索引 #{createdIndex} 超出 ushort 范围。");

            var native = ClientObjectManager.Instance.GetObjectByIndex((ushort)createdIndex);
            if (native.IsNull())
                throw new InvalidOperationException($"生成实体失败：返回的实体索引 #{createdIndex} 对应空指针。");

            var character = new Character(native, _host);
            _localEntityIndexes[character.Address] = (ushort)createdIndex;

            return character;
        }

        private const CharacterSetupContainer.CopyFlags DefaultPlayerCopyFlags =
            CharacterSetupContainer.CopyFlags.ClassJob |
            CharacterSetupContainer.CopyFlags.Position |
            CharacterSetupContainer.CopyFlags.Name;

        public Character SpawnPlayer(
            Job job,
            Character source = null,
            CharacterSetupContainer.CopyFlags copyFlags = DefaultPlayerCopyFlags)
        {
            if (source != null && source.Native.ObjectKind != ObjectKind.Pc)
                throw new ArgumentException("实体源必须为玩家角色", nameof(source));

            source = source ?? _host.EntityProvider.GetMyself()
                ?? throw new ArgumentException("未找到自身实体", nameof(source));

            var player = CreateBattleCharacter();

            player.Native.CharacterSetup.CopyFromCharacter(source.Native, copyFlags);
            player.Native.ObjectKind.Set(ObjectKind.Pc);
            player.Id = GetNextPlayerId();
            player.Job = job;

            return player;
        }

        public Character SpawnBNpc(uint bNpcBaseId, uint bNpcNameId, byte level)
        {
            var bnpc = CreateBattleCharacter();

            bnpc.Native.CharacterSetup.SetupBNpc(bNpcBaseId, bNpcNameId);
            bnpc.Native.ObjectKind.Set(ObjectKind.BattleNpc);
            bnpc.Id = GetNextNonPlayerId();
            bnpc.Level = level;
            bnpc.Native.Battalion.Set(4);
            bnpc.Native.IsHostile.Set(true);
            bnpc.Native.InCombat.Set(true);
            return bnpc;
        }

        private static readonly object _idLock = new object();
        private static ushort _currentPlayerIdIndex = 0;
        private static ushort _currentNonPlayerIdIndex = 0;

        private uint GetNextPlayerId()
        {
            lock (_idLock)
            {
                var newIndex = unchecked(++_currentPlayerIdIndex);
                return 0x10FF0000u + newIndex;
            }
        }

        private uint GetNextNonPlayerId()
        {
            lock (_idLock)
            {
                var newIndex = unchecked(++_currentNonPlayerIdIndex);
                return 0x40FF0000u + newIndex;
            }
        }

        public void Delete(EntityBase entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.Native.IsNull())
                throw new ArgumentException("实体的原生对象为空", nameof(entity));

            if (!_localEntityIndexes.TryRemove(entity.Address, out ushort createdIndex))
                return;

            ClientObjectManager.Instance.DeleteObjectByIndex(createdIndex, 0);
        }

        public EventObject SpawnEObj(EObjData data)
        {
            var packet = (SpawnObjectPacket)data;
            packet.EntityId = GetNextNonPlayerId();
            _ = PacketDispatcher.HandleSpawnObjectPacket(packet.EntityId, packet);

            // 可以用 Index 优化，避免遍历查找
            return _host.EntityProvider.GetEventObjs().FirstOrDefault(e => e.Native.EntityId == packet.EntityId) 
                ?? throw new InvalidOperationException($"生成实体失败：未找到 EntityId={packet.EntityId:X8} 的 EObj 实体。");
        }
    }

    public class EObjData
    {
        public byte Index;
        public byte TargetableFlags;
        public bool Visible;
        public uint BaseId;
        public uint LayoutId;
        public uint EventId;
        public uint GimmickId;
        public float Radius = 1f;
        public ushort FateId;
        public byte EventState;
        public ushort SharedTimelineState;
        public uint SharedGroupState;
        public Vector3 Pos;
        public float Heading;

        public static explicit operator SpawnObjectPacket(EObjData data)
        {
            return new SpawnObjectPacket
            {
                ObjectIndex = data.Index,
                ObjectKind = (byte)ObjectKind.EventObj,
                TargetableStatus = data.TargetableFlags,
                Visibility = (byte)(data.Visible ? 1 : 0),
                BaseId = data.BaseId,
                LayoutId = data.LayoutId,
                EventId = data.EventId,
                OwnerId = 0xE0000000,
                GimmickId = data.GimmickId,
                Radius = data.Radius,
                Rotation = PacketCodec.EncodeUShortCoord(data.Heading),
                FateId = data.FateId,
                EventState = data.EventState,
                SharedTimelineState = data.SharedTimelineState,
                SharedGroupState = data.SharedGroupState,
                PositionX = data.Pos.X,
                PositionY = data.Pos.Z, // flip
                PositionZ = data.Pos.Y
            };
        }
    }
}