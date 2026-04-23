using System;
using System.Numerics;
using Simulant.Game;
using Simulant.Game.FFCS.Client.Game.Object;
using NativeEventObject = Simulant.Game.FFCS.Client.Game.Object.EventObject;

namespace Simulant.Core.Entity
{
    public class EventObject : EntityBase
    {
        protected override GameObject NativeGameObject => Native.Ptr.As<GameObject>();
        internal new NativeEventObject Native { get; }

        public EventObject(NativeEventObject native)
        {
            Native = native;
        }

        public void PlayAnimation(uint actionId, ulong unknown)
            => Native.PlayAnimation(Native.EntityId, actionId, unknown);
    }
}