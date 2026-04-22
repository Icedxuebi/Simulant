using Simulant.ACT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Simulant.Game
{
    internal static class AddressStore
    {
        //[SigPattern(null)]
        //public static IntPtr PlayActionTimelineFuncPtr { get; set; }

        [SigPattern("E8 * * * * 48 8D 0D ? ? ? ? E8 ? ? ? ? FF C6")]
        public static IntPtr KnockbackFuncPtr { get; set; }

    }

}