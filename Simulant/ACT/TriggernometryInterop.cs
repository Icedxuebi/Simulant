using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Advanced_Combat_Tracker;

namespace Simulant.ACT
{
    public static class TriggernometryInterop
    {
        private static MethodInfo _getEntitiesMethod;
        private static object _realPluginInstance;

        public static void Init()
        {
            var asm = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(x => x.GetName().Name == "TriggernometryPlugin")
                ?? throw new Exception($"[{nameof(TriggernometryInterop)}] 未找到 TriggernometryPlugin 程序集。");

            var entityType = asm.GetType("Triggernometry.FFXIV.Entity")
                ?? throw new Exception($"[{nameof(TriggernometryInterop)}] 未找到 Triggernometry.FFXIV.Entity。");

            _getEntitiesMethod = entityType.GetMethod(
                "GetEntities",
                BindingFlags.Public | BindingFlags.Static,
                null,
                new[] { typeof(bool) },
                null)
                ?? throw new Exception($"[{nameof(TriggernometryInterop)}] 未找到 Entity.GetEntities(bool)。");

            var realPluginType = asm.GetType("Triggernometry.Core.RealPlugin")
                ?? throw new Exception($"[{nameof(TriggernometryInterop)}] 未找到 Triggernometry.Core.RealPlugin。");

            _realPluginInstance = realPluginType
                .GetProperty("Instance", BindingFlags.Public | BindingFlags.Static)
                .GetValue(null, null);
        }

        public static void DeInit()
        {
            _getEntitiesMethod = null;
            _realPluginInstance = null;
        }

        public static IEnumerable<IntPtr> GetEntityPtrs()
        {
            var result = _getEntitiesMethod.Invoke(null, new object[] { true });

            foreach (dynamic entity in (IEnumerable)result)
            {
                IntPtr ptr = entity.Address;
                if (ptr != IntPtr.Zero)
                    yield return ptr;
            }
        }

        public static void InvokeNamedCallback(string name, string val)
        {
            dynamic plugin = _realPluginInstance;
            plugin.InvokeNamedCallback(name, val ?? string.Empty);
        }
    }
}