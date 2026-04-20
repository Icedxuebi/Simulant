using System;
using Simulant.ACT;

namespace Simulant.Game
{
    internal sealed class GameFunctions
    {
        private readonly NamazuInterop _namazu;
        private readonly AddressStore _addr;

        public GameFunctions(NamazuInterop namazu, AddressStore addr)
        {
            _namazu = namazu;
            _addr = addr;
        }

        public bool IsReady
        {
            get
            {
                return _namazu != null
                    && _namazu.IsReady
                    && _namazu.Plugin != null
                    && _namazu.Plugin.Memory != null;
            }
        }

        public void Scan()
        {
            if (!IsReady)
                return;

            if (_addr.PlayActionTimelinePtr == IntPtr.Zero)
            {
                _addr.PlayActionTimelinePtr = _namazu.Plugin.SigScanner.TryScan(
                    "",
                    "PlayActionTimeline");
            }

            if (_addr.KnockbackPtr == IntPtr.Zero)
            {
                _addr.KnockbackPtr = _namazu.Plugin.SigScanner.TryScan(
                    "",
                    "Knockback");
            }
        }

        public void PlayActionTimeline(IntPtr entityPtr, ushort timelineId, byte param1, byte param2)
        {
            if (!IsReady || entityPtr == IntPtr.Zero || _addr.PlayActionTimelinePtr == IntPtr.Zero)
                return;

            _namazu.Plugin.Memory.CallInjected64(
                _addr.PlayActionTimelinePtr,
                entityPtr,
                timelineId,
                param1,
                param2);
        }

        public void PlayActionTimeline(IntPtr entityPtr, ushort timelineId)
        {
            PlayActionTimeline(entityPtr, timelineId, 0, 0);
        }

        public void Knockback(IntPtr entityPtr, float angle)
        {
            if (!IsReady || entityPtr == IntPtr.Zero || _addr.KnockbackPtr == IntPtr.Zero)
                return;

            _namazu.Plugin.Memory.CallInjected64(
                _addr.KnockbackPtr,
                entityPtr,
                angle);
        }
    }
}