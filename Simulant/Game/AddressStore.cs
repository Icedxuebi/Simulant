using Simulant.ACT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Simulant.Game
{
    internal static class AddressStore
    {
        [SigPattern(null)]
        public static IntPtr PlayActionTimelineFuncPtr { get; set; }

        [SigPattern("E8 * * * * 48 8D 0D ? ? ? ? E8 ? ? ? ? FF C6")]
        public static IntPtr KnockbackFuncPtr { get; set; }

        public static Dictionary<string, string> Scan()
        {
            var ptrProps = typeof(AddressStore).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                .Where(p => p.PropertyType == typeof(IntPtr) && p.GetCustomAttribute<SigPatternAttribute>() != null)
                .ToList();

            ClearPtrs(ptrProps);

            var scanner = NamazuInterop.Plugin?.SigScanner;
            if (!NamazuInterop.IsReady || scanner == null)
                throw new Exception("[AddressStore] Namazu scanner 未就绪。");

            Dictionary<string, string> result = new Dictionary<string, string>();

            ScanSpecialPtrs(scanner, result);
            ScanGeneralPtrs(ptrProps, scanner, result);

            foreach (var ptrProp in ptrProps)
            {
                var name = ptrProp.Name;
                if (result.ContainsKey(name))
                    continue;

                result[name] = (IntPtr)ptrProp.GetValue(null) == IntPtr.Zero
                    ? "无可用内存签名，且未设置特殊扫描方法" : null;
            }

            return result;
        }

        private static void ScanSpecialPtrs(NamazuScanner scanner, Dictionary<string, string> result)
        {
            // if needed
        }

        private static void ScanGeneralPtrs(List<PropertyInfo> ptrProps, NamazuScanner scanner, Dictionary<string, string> result)
        {
            foreach (var ptrProp in ptrProps)
            {
                if ((IntPtr)ptrProp.GetValue(null) != IntPtr.Zero) // scanned special case
                    continue;

                var sigs = ptrProp.GetCustomAttribute<SigPatternAttribute>().Signatures;
                if (sigs == null || sigs.Length == 0)
                    continue;

                var ptr = scanner.TryScanMultiple(sigs, ptrProp.Name);
                if (ptr == IntPtr.Zero)
                {
                    result[ptrProp.Name] = "未能找到与签名对应的内存地址";
                }
                else
                {
                    ptrProp.SetValue(null, ptr);
                    result[ptrProp.Name] = null;
                }
            }
        }

        private static void ClearPtrs(List<PropertyInfo> ptrProps)
        {
            foreach (var ptrProp in ptrProps)
            {
                ptrProp.SetValue(null, IntPtr.Zero);
            }
        }

        [AttributeUsage(AttributeTargets.Property)]
        internal sealed class SigPatternAttribute : Attribute
        {
            public string[] Signatures { get; }

            public SigPatternAttribute(params string[] signatures)
            {
                Signatures = signatures;
            }
        }
    }
}