using Simulant.Game;
using Simulant.Game.FFCS.Client.Game.Character;
using Simulant.Game.FFCS.Client.Game.Object;
using NativeCharacter = Simulant.Game.FFCS.Client.Game.Character.Character;

namespace Simulant.Core.Entity
{
    public class Character : EntityBase
    {
        private BaseTimelineState? _originalBaseTimelineState;

        protected override GameObject NativeGameObject => Native.Ptr.As<GameObject>();
        public new NativeCharacter Native { get; }
        public Character(NativeCharacter native)
        {
            Native = native;
        }

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
        }

        public void PlayBaseTimeline(ushort timelineId, bool interrupt = true)
        {
            if (timelineId == 0) return;

            if (_originalBaseTimelineState == null)
            {
                _originalBaseTimelineState = new BaseTimelineState(
                    Native.Mode,
                    Native.ModeParam,
                    Native.Timeline.BaseOverride);
            }

            Native.SetMode(CharacterModes.AnimLock, 0);
            Native.Timeline.BaseOverride.Set(timelineId);

            if (interrupt)
                Native.Timeline.TimelineSequencer.PlayTimeline(timelineId);
        }

        public void StopBaseTimeline()
        {
            if (_originalBaseTimelineState == null)
                return;

            var state = _originalBaseTimelineState.Value;

            Native.Timeline.BaseOverride.Set(state.BaseOverride);
            Native.Mode.Set(state.Mode);
            Native.ModeParam.Set(state.ModeParam);

            _originalBaseTimelineState = null;

            Native.Timeline.TimelineSequencer.PlayTimeline(3);
        }

        public void ResetBaseTimelineState()
        {
            _originalBaseTimelineState = null;
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
    }
}