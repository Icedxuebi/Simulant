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

        public bool IsReady => Memory != null && Memory.IsProcessOpen;

        private readonly Func<object> _getNamazuUi;
        public object PluginUI => _getNamazuUi();

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

        public bool IsCN => _plugin.IsCN;
        public IntPtr FrameworkPtr => _plugin.FrameworkPtr;
        public Dictionary<string, bool> ActionEnabled => _plugin.ActionEnabled;

        public void DoAction(string command, string payload)
            => _plugin.DoAction(command, payload);

        /// <summary>
        ///   尝试在 GreyMagic FrameLock 窗口内同步执行 Call 等操作。<br />
        ///   若短期内多次调用，会自动尝试在同一帧内调用，注意避免传入高延迟动作阻塞 UI。<br />
        /// </summary>
        /// <remarks>
        ///   用于降低连续直接调用 CallInjected64 时因等待 hook 入口再次触发而产生的高延迟。<br />
        ///   如果只是调用函数，可直接使用 Call()。
        /// </remarks>
        public void ExecuteInFrameLock(Action action)
            => _plugin.ExecuteInFrameLock(action);

        /// <summary>
        ///   尝试在 GreyMagic FrameLock 窗口内同步执行 Call 等操作。<br />
        ///   若短期内多次调用，会自动尝试在同一帧内调用，注意避免传入高延迟动作阻塞 UI。<br />
        /// </summary>
        /// <remarks>
        ///   用于降低连续直接调用 CallInjected64 时因等待 hook 入口再次触发而产生的高延迟。<br />
        ///   如果只是调用函数，可直接使用 Call<T>()。
        /// </remarks>
        public T ExecuteInFrameLock<T>(Func<T> func)
            => _plugin.ExecuteInFrameLock<T>(func);

        /// <summary> 直接通过 GreyMagic 在指定的函数地址调用 CallInjected64，不经过 FrameLock 调度器。</summary>
        public void DirectCall(IntPtr ptr, params object[] args)
            => _plugin.DirectCall(ptr, args);

        /// <summary> 直接通过 GreyMagic 在指定的函数地址调用 CallInjected64，不经过 FrameLock 调度器。</summary>
        public T DirectCall<T>(IntPtr ptr, params object[] args) where T : struct
            => _plugin.DirectCall<T>(ptr, args);

        /// <summary> 使用自动复用的 FrameLock 窗口，在指定的函数地址调用 CallInjected64。</summary>
        public void Call(IntPtr ptr, params object[] args)
            => _plugin.Call(ptr, args);

        /// <summary> 使用自动复用的 FrameLock 窗口，在指定的函数地址调用 CallInjected64。</summary>
        public T Call<T>(IntPtr ptr, params object[] args) where T : struct
            => _plugin.Call<T>(ptr, args);
    }
}