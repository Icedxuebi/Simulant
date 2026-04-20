using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Simulant.ACT
{
    /// <summary>
    /// Wrapper for PostNamazu.PostNamazu
    /// </summary>
    public class NamazuPlugin
    {
        private readonly dynamic _plugin;

        private GreyMagicExternalProcessMemory _memory;
        public GreyMagicExternalProcessMemory Memory
        {
            get
            {
                var current = _plugin.Memory;
                if (_memory?.RawMemory != current)
                {
                    _memory = current == null ? null : new GreyMagicExternalProcessMemory(current);
                }
                return _memory;
            }
        }

        private NamazuScanner _sigScanner;
        public NamazuScanner SigScanner
        {
            get
            {
                var current = _plugin.SigScanner;
                if (_sigScanner?.RawScanner != current)
                {
                    _sigScanner = current == null ? null : new NamazuScanner(current);
                }
                return _sigScanner;
            }
        }

        public bool IsReady
        {
            get { return Memory != null && Memory.IsProcessOpen; }
        }

        private readonly Func<object> _getNamazuUi;
        public object PluginUI
        {
            get { return _getNamazuUi(); }
        }

        public NamazuPlugin(object plugin)
        {
            _plugin = plugin ?? throw new ArgumentNullException(nameof(plugin));

            var pluginType = plugin.GetType();
            var fi = pluginType.GetField("PluginUi", BindingFlags.Instance | BindingFlags.NonPublic)
                ?? pluginType.GetField("PluginUI", BindingFlags.Instance | BindingFlags.NonPublic)
                ?? throw new MissingFieldException($"[{nameof(NamazuInterop)}] 未找到 PluginUi。");

            _getNamazuUi = () => fi.GetValue(_plugin);
        }

        public object GetOriginalModuleByName(string moduleName)
        {
            var modulesField = _plugin.GetType().GetField("Modules", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                ?? throw new Exception($"[{nameof(NamazuInterop)}] 未找到 Modules 列表。");

            var modules = modulesField.GetValue(_plugin) as IList
                ?? throw new Exception($"[{nameof(NamazuInterop)}] Modules 实例不是列表。");

            return modules.Cast<object>().FirstOrDefault(m => m.GetType().Name.Equals(moduleName, StringComparison.OrdinalIgnoreCase));
        }

        public Dictionary<string, bool> ActionEnabled
        {
            get { return _plugin.ActionEnabled; }
        }

        public void DoAction(string command, string payload)
        {
            _plugin.DoAction(command, payload);
        }

        public bool IsCN
        {
            get { return _plugin.IsCN; }
        }

        public IntPtr FrameworkPtr
        {
            get { return _plugin.FrameworkPtr; }
        }
    }
}