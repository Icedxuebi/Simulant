using System;

namespace Simulant.Game
{
    internal sealed class AddressStore
    {
        public IntPtr PlayActionTimelinePtr { get; set; }
        public IntPtr KnockbackPtr { get; set; }

        public void Clear()
        {
            PlayActionTimelinePtr = IntPtr.Zero;
            KnockbackPtr = IntPtr.Zero;
        }
    }
}