using Simulant.ACT;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Simulant.Game
{
    public static class IntPtrExtensions
    {
        public static bool IsZero(this IntPtr ptr) => ptr == IntPtr.Zero;

        public static void ThrowIfZero(this IntPtr ptr, string callerName, int offset, string opDesc)
        {
            if (ptr == IntPtr.Zero)
                throw new InvalidOperationException($"{callerName} 在 0x{offset:X} 处{opDesc}时指针为空。");
        }

        public static T Read<T>(this IntPtr ptr, int offset = 0, 
            [CallerMemberName] string callerName = "") where T : struct
        {
            ptr.ThrowIfZero(callerName, offset, "读取");
            return NamazuInterop.Plugin.Memory.Read<T>(ptr + offset);
        }

        public static IntPtr ReadPtr(this IntPtr ptr, int offset = 0,
            [CallerMemberName] string callerName = "")
        {
            ptr.ThrowIfZero(callerName, offset, "读取 IntPtr ");
            return NamazuInterop.Plugin.Memory.Read<IntPtr>(ptr + offset);
        }

        public static TMemoryObject As<TMemoryObject>(this IntPtr ptr, int offset = 0, 
            [CallerMemberName] string callerName = "") where TMemoryObject : struct, IMemoryObject
        {
            if (ptr == IntPtr.Zero)
                throw new InvalidOperationException($" 时指针为空。");
            ptr.ThrowIfZero(callerName, offset, $"读取 {typeof(TMemoryObject).Name} ");
            return new TMemoryObject { Ptr = ptr + offset };
        }

        public static void Write<T>(this IntPtr ptr, T value, int offset = 0, 
            [CallerMemberName] string callerName = "") where T : struct
        {
            ptr.ThrowIfZero(callerName, offset, "写入");
            NamazuInterop.Plugin.Memory.Write<T>(ptr + offset, value);
        }

        public static void Call(this IntPtr ptr, params object[] args)
        {
            NamazuInterop.Plugin.Memory.CallInjected64(ptr, args);
        }

        public static T Call<T>(this IntPtr ptr, params object[] args) where T : struct
        {
            ptr.ThrowIfZero("Call", 0, "调用函数");
            return NamazuInterop.Plugin.Memory.CallInjected64<T>(ptr, args);
        }

        public static IntPtr GetVFuncPtr(this IntPtr objectPtr, int idx, [CallerMemberName] string callerName = "")
        {
            objectPtr.ThrowIfZero(callerName, 0, "获取虚函数指针");
            var vTable = objectPtr.Read<IntPtr>().As<VTable>();
            return vTable[idx];
        }

        public static void CallVFunc(this IntPtr objectPtr, int idx, params object[] args)
        {
            var vFuncPtr = GetVFuncPtr(objectPtr, idx);
            vFuncPtr.Call(new object[] { objectPtr }.Concat(args).ToArray());
        }

        public static T CallVFunc<T>(this IntPtr objectPtr, int idx, params object[] args) where T : struct
        {
            var vFuncPtr = GetVFuncPtr(objectPtr, idx);
            return vFuncPtr.Call<T>(new object[] { objectPtr }.Concat(args).ToArray());
        }
    }
}
