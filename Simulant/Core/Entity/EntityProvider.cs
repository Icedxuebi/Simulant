using Simulant.Game;
using Simulant.Game.FFCS.Client.Game.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using Simulant.ACT;
using NativeCharacter = Simulant.Game.FFCS.Client.Game.Character.Character;
using NativeEventObject = Simulant.Game.FFCS.Client.Game.Object.EventObject;

namespace Simulant.Core.Entity
{
    public sealed class EntityProvider
    {
        private readonly PluginHost _host;
        public EntityProvider(PluginHost host)
        {
            _host = host;
        }

        public Character GetMyself()
            => GetCharacters().FirstOrDefault();

        public IEnumerable<Character> GetCharacters()
            => GetEntities().OfType<Character>();

        public IEnumerable<EventObject> GetEventObjs()
            => GetEntities().OfType<EventObject>();

        private const uint IgnoredEntityId = 0xE0000000;
        public IEnumerable<EntityBase> GetEntities() 
        {
            var seenEntityIds = new HashSet<uint>();

            foreach (var nativePtr in TriggernometryInterop.GetEntityPtrs()) // 先直接调用 Triggernometry 现成的方法了
            {
                if (nativePtr == IntPtr.Zero)
                    continue;

                var gameObject = nativePtr.As<GameObject>();
                
                var entityId = gameObject.EntityId;
                if (entityId == 0 || entityId == IgnoredEntityId || !seenEntityIds.Add(entityId))
                    continue;

                var entity = WrapEntity(nativePtr, gameObject.ObjectKind); // only PC / BNpc / EObj are returned, others are null
                if (entity != null)
                    yield return entity;
            }
        }

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