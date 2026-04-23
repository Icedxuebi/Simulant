using Simulant.Game;
using Simulant.Game.FFCS.Client.Game.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using NativeCharacter = Simulant.Game.FFCS.Client.Game.Character.Character;
using NativeEventObject = Simulant.Game.FFCS.Client.Game.Object.EventObject;

namespace Simulant.Core.Entity
{
    public sealed class EntityProvider
    {
        private readonly ClientObjectManager _objectManager;

        public EntityProvider()
        {
            _objectManager = ClientObjectManager.Instance;
        }

        public IEnumerable<EntityBase> GetEntities()
            => GetEntities(_ => true);

        public IEnumerable<Character> GetCharacters()
            => GetEntities(IsCharacterKind).Cast<Character>();

        public IEnumerable<EventObject> GetEventObjs()
            => GetEntities(k => k == ObjectKind.EventObj).Cast<EventObject>();

        /// <summary> <see cref="ClientObjectManager.BattleCharaList"/> 内存布局相关常量 </summary>
        private const int BattleCharaListOffset = 0x10;
        private const int BattleCharaEntrySize = 0x10;
        private const int BattleCharaPtrOffset = 0x00;
        private const int BattleCharaKindOffset = 0x08;
        private const uint IgnoredEntityId = 0xE0000000;
        public IEnumerable<EntityBase> GetEntities(Func<ObjectKind, bool> filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            // 由于远端读取内存有 ~1 μs 的延迟，不用 Native 结构直接遍历远端内存，
            // 而是一次性读取整个 BattleCharaList 到本地内存再处理。

            var seenEntityIds = new HashSet<uint>();
            var bytes = _objectManager.Ptr.ReadBytes(
                ClientObjectManager.BattleCharaList.Count * BattleCharaEntrySize,
                BattleCharaListOffset);

            for (var i = 0; i < ClientObjectManager.BattleCharaList.Count; i++)
            {
                var entryOffset = i * BattleCharaEntrySize;

                var nativePtr = ReadEntryPtr(bytes, entryOffset);
                if (nativePtr == IntPtr.Zero)
                    continue;

                // 在遍历读取指针前，用 Kind 筛选掉不需要的条目，减少不必要的远端读取 Id。
                var kind = ReadEntryKind(bytes, entryOffset);
                if (!filter(kind))
                    continue;

                var entityId = nativePtr.As<GameObject>().EntityId;
                if (entityId == 0 || entityId == IgnoredEntityId || !seenEntityIds.Add(entityId))
                    continue;

                var entity = WrapEntity(nativePtr, kind);
                if (entity != null)
                    yield return entity;
            }
        }

        private static IntPtr ReadEntryPtr(byte[] bytes, int entryOffset)
            => new IntPtr(BitConverter.ToInt64(bytes, entryOffset + BattleCharaPtrOffset));

        private static ObjectKind ReadEntryKind(byte[] bytes, int entryOffset)
            => (ObjectKind)BitConverter.ToInt32(bytes, entryOffset + BattleCharaKindOffset);

        private static bool IsCharacterKind(ObjectKind kind)
        {
            return kind == ObjectKind.Pc
                || kind == ObjectKind.BattleNpc;
        }

        private static EntityBase WrapEntity(IntPtr nativePtr, ObjectKind kind)
        {
            if (kind == ObjectKind.EventObj)
                return WrapEventObj(nativePtr);

            if (IsCharacterKind(kind))
                return WrapCharacter(nativePtr);

            return null;
        }

        private static Character WrapCharacter(IntPtr nativePtr)
            => new Character(nativePtr.As<NativeCharacter>());

        private static EventObject WrapEventObj(IntPtr nativePtr)
            => new EventObject(nativePtr.As<NativeEventObject>());
    }
}