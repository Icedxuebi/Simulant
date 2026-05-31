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
        private readonly PluginHost _host;

        public EventObject(NativeEventObject native, PluginHost host)
        {
            Native = native;
            _host = host;
        }

        // 写入值 + 某些条件下通知更新
        // 如 O11s EObj 塔的有/无人踩塔特效切换通过此包不断发送而更新
        // 实际发包还有个 a2 如 0x80050287，不过函数里没有用到
        // 函数里还判断了如果 a3 不是 0 则走一个与 a4 相关的分支，不过对于战斗中的 EObj 暂时没有看到 a3 不为 0 的情况
        public void SetSharedTimelineState(ushort sharedTimelineState) 
            => ActorControl(0x0199, sharedTimelineState);

        // 更准确地：PlaySharedTimelineAnimation
        public void PlayAnimation(ushort bitMask)
            => PlayAnimation(0, bitMask, 0);

        public void PlayAnimation(ushort sharedTimelineState, ushort bitMask, ulong context = 0)
            => Native.PlayAnimation(sharedTimelineState, bitMask, context);

    }
}