using Simulant.Game;
using Simulant.Game.FFCS.Client.Game.Character;
using Simulant.Game.FFCS.Client.Game.Object;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Simulant.Core.Entity
{
    public sealed class EntitySpawner
    {
        private readonly PluginHost _host;
        private readonly byte _level;

        public EntitySpawner(PluginHost host, byte level)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
            _level = level;
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

            var character = new Character(native);
            _localEntityIndexes[character.Address] = (ushort)createdIndex;

            return character;
        }

        private const CharacterSetupContainer.CopyFlags DefaultPlayerCopyFlags =
            CharacterSetupContainer.CopyFlags.ClassJob |
            CharacterSetupContainer.CopyFlags.Position |
            CharacterSetupContainer.CopyFlags.Name;

        public Character SpawnPlayer(
            byte job,
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
            player.Level = _level;
            player.ClassJob = job;

            return player;
        }

        public Character SpawnBNpc(uint bNpcBaseId, uint bNpcNameId = 0)
        {
            var bnpc = CreateBattleCharacter();

            bnpc.Native.CharacterSetup.SetupBNpc(bNpcBaseId, bNpcNameId);
            bnpc.Native.ObjectKind.Set(ObjectKind.BattleNpc);
            bnpc.Id = GetNextNonPlayerId();
            bnpc.Level = _level;

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
    }
}