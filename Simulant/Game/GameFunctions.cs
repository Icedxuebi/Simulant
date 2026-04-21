using Simulant.ACT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using static Simulant.Game.AddressStore;

namespace Simulant.Game
{
    public static class GameFunctions
    {

        public static IntPtr Knockback(IntPtr entityPtr, float angle, float distance, float duration, byte a5 = 0, int a6 = 0)
        {
            CheckStatus(KnockbackFuncPtr);
            return KnockbackFuncPtr.Call<IntPtr>(entityPtr, angle, distance, duration, a5, a6);
        }

        #region Common

        private static void CheckStatus([CallerMemberName] string callerNameAuto = null)
            => CheckStatusCore(callerNameAuto);
        private static void CheckStatus(IntPtr ptr1, [CallerMemberName] string callerNameAuto = null)
            => CheckStatusCore(callerNameAuto, ptr1);
        private static void CheckStatus(IntPtr ptr1, IntPtr ptr2, [CallerMemberName] string callerNameAuto = null)
            => CheckStatusCore(callerNameAuto, ptr1, ptr2);
        private static void CheckStatus(IntPtr ptr1, IntPtr ptr2, IntPtr ptr3, [CallerMemberName] string callerNameAuto = null)
            => CheckStatusCore(callerNameAuto, ptr1, ptr2, ptr3);
        private static void CheckStatus(IntPtr ptr1, IntPtr ptr2, IntPtr ptr3, IntPtr ptr4, [CallerMemberName] string callerNameAuto = null)
            => CheckStatusCore(callerNameAuto, ptr1, ptr2, ptr3, ptr4);
        private static void CheckStatusCore([CallerMemberName] string callerNameAuto = null, params IntPtr[] ptrs)
        {
            if (!NamazuInterop.IsReady || NamazuInterop.Plugin?.Memory == null)
                throw new InvalidOperationException($"{callerNameAuto} 函数执行时 NamazuInterop 未就绪。");

            if (ptrs.Any(ptr => ptr == IntPtr.Zero))
                throw new InvalidOperationException($"{callerNameAuto} 函数执行所需的指针为空。");
        }

        private static void Call(this IntPtr ptr, params object[] args)
            => NamazuInterop.Plugin.Memory.CallInjected64(ptr, args);

        private static T Call<T>(this IntPtr ptr, params object[] args) where T : struct
            => NamazuInterop.Plugin.Memory.CallInjected64<T>(ptr, args);

        #endregion Common
    }
}