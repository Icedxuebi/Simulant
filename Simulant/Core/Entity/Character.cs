using Simulant.ACT;
using Simulant.Game;
using Simulant.Game.ExtractedCsv;
using Simulant.Game.FFCS.Client.Game.Character;
using Simulant.Game.FFCS.Client.Game.Object;
using System.Numerics;
using System.Threading.Tasks;
using ActionRow = Simulant.Game.ExtractedCsv.Rows.Action;
using NativeCharacter = Simulant.Game.FFCS.Client.Game.Character.Character;

namespace Simulant.Core.Entity
{
    public class Character : EntityBase
    {
        private readonly PluginHost _host;

        protected override GameObject NativeGameObject => Native.Ptr.As<GameObject>();
        public new NativeCharacter Native { get; }
        public Character(NativeCharacter native, PluginHost host)
        {
            Native = native;
            _host = host;
        }

        /// <summary>
        /// 用于控制假实体移动的坐标。
        /// </summary>
        public Vector2 TargetPos { get; set; }

        public uint HP
        {
            get => Native.Health;
            set => Native.Health.Set(value);
        }

        public uint MaxHP
        {
            get => Native.MaxHealth;
            set => Native.MaxHealth.Set(value);
        }

        public uint MP
        {
            get => Native.Mana;
            set => Native.Mana.Set(value);
        }

        public uint MaxMp
        {
            get => Native.MaxMana;
            set => Native.MaxMana.Set(value);
        }

        public byte Level
        {
            get => Native.Level;
            set => Native.Level.Set(value);
        }

        public byte ClassJob
        {
            get => Native.ClassJob;
            set => Native.ClassJob.Set(value);
        }

        public CharacterModes Mode
        {
            get => Native.Mode;
            // set => Native.Mode.Set(value); should set by SetMode method?
        }

        public byte ModeParam
        {
            get => Native.ModeParam;
            // set => Native.ModeParam.Set(value);
        }

        public void SetMode(CharacterModes mode, byte modeParam = 0)
            => Native.SetMode(mode, modeParam);

        public bool HasStatus(uint statusId)
            => Native.HasStatus(statusId);

        public void Knockback(float angle, float distance, float duration, byte a5 = 0, int a6 = 0)
            => Native.Knockback(angle, distance, duration, a5, a6);

        public void PlayBlendTimeline(ushort timelineId)
        {
            if (timelineId == 0) return;
            Native.Timeline.TimelineSequencer.PlayTimeline(timelineId);
            _host.LogVerbose($"PlayTimeline: {timelineId}");
        }

        public void PlayBaseTimeline(ushort timelineId, bool interrupt = true)
        {
            if (timelineId == 0) return;

            Native.SetMode(CharacterModes.AnimLock, 0);
            Native.Timeline.BaseOverride.Set(timelineId);

            if (!interrupt) return;
            
            Native.Timeline.TimelineSequencer.PlayTimeline(timelineId);
            _host.LogVerbose($"PlayTimeline: {timelineId}");
        }

        private readonly struct BaseTimelineState
        {
            public readonly CharacterModes Mode;
            public readonly byte ModeParam;
            public readonly ushort BaseOverride;

            public BaseTimelineState(CharacterModes mode, byte modeParam, ushort baseOverride)
            {
                Mode = mode;
                ModeParam = modeParam;
                BaseOverride = baseOverride;
            }
        }

        private static ActionRow ResolveActionData(uint actionId)
            => CsvManager.Instance.Get<ActionRow>().TryGetValue((int)actionId, out var action) ? action : null;

        public void Cast(uint actionId, float? omenDelay = 0)
        {
            var action = ResolveActionData(actionId);
            if (action == null) return;

            var castTime = action.CastTime;

            // 用 CastInfo 控制读条开始，可以自动创建特效、显示咏唱进度条
            SetCastInfo(actionId, castTime);

            // 如果技能表里有 omen：
            var omenId = action.OmenId;
            if (omenId > 0 && omenDelay.HasValue)
                PlayOmen(action, omenDelay.Value);

            // 读条结束时播放对应 timeline
            QueueExecute(action, castTime);

            _host.LogVerbose($"Casting action: {actionId} ({castTime:0.0} s)");
        }

        public void SetCastInfo(uint actionId, float castTime, byte actionType = 1)
        {
            var castInfo = Native.CastInfo;
            castInfo.IsCasting.Set(true);
            castInfo.ActionType.Set(actionType); // 普通技能
            castInfo.ActionId.Set(actionId);
            //castInfo.TargetId.Set(0xE000_0000);
            castInfo.CurrentCastTime.Set(0);
            castInfo.BaseCastTime.Set(castTime);
            castInfo.TotalCastTime.Set(castTime);
        }

        public void PlayOmen(ActionRow action, float omenDelay)
        {
            omenDelay = System.Math.Max(0, omenDelay);
            var sustain = action.CastTime - omenDelay; // 实际游戏考虑到网络延迟，会统一缩短 0.3 s，但这里本地判定逻辑所以不延迟
            if (sustain < 0) return;

            var rawX = action.ScaleX;
            var rawY = action.ScaleY;
            var center = Pos3D;
            var heading = Heading;
            float x, y;
            switch (action.ShapeType)
            {
                case 2: // 圆
                case 10: // 圆环
                case 13: // 扇形
                    x = y = rawY;
                    break;
                case 3: // 扇形 + R
                case 5: // 圆形 + R
                    x = y = rawY + Native.HitboxRadius;
                    break;
                case 12: // 矩形
                case 11: // 十字
                case 14: // 三角
                    x = rawX;
                    y = rawY;
                    break;
                case 4: // 矩形 + R
                    x = rawX;
                    y = rawY + Native.HitboxRadius;
                    break;
                default:
                    return;
            }
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Tag: Simulant");
            sb.AppendLine($"Omen: {action.Omen.Name}");
            sb.AppendLine($"Delay: {omenDelay}");
            sb.AppendLine($"t: {sustain}"); 
            sb.AppendLine($"O: {center.X}, {center.Y}, {center.Z}");
            sb.AppendLine($"θ: {heading}");
            sb.AppendLine($"Scale: {x}, {y}, 1");
            TriggernometryInterop.InvokeNamedCallback("PictoACT", sb.ToString());
        }

        public void Execute(uint actionId)
        {
            var action = ResolveActionData(actionId);
            if (action == null) return;

            Execute(action);
        }

        private void Execute(ActionRow action)
        {
            var timelineEndId = action.AnimationEndId;
            if (timelineEndId >= 0)
            {
                Native.Timeline.BaseOverride.Set(0);
                Native.SetMode(CharacterModes.Normal, 0);
                PlayBlendTimeline((ushort)timelineEndId);
            }

            _host.LogVerbose($"Executed action: {action.Index}; TimelineEnd: {timelineEndId}");
        }

        private void QueueExecute(ActionRow action, float delay)
        {
            Task.Run(async () =>
            {
                if (delay > 0)
                    await Task.Delay((int)(delay * 1000));

                Execute(action);
            });
        }

    }
}