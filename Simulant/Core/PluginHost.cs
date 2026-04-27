using Advanced_Combat_Tracker;
using Simulant.ACT;
using Simulant.Core.Entity;
using Simulant.Core.Zone;
using Simulant.Game;
using Simulant.Game.ExtractedCsv;
using Simulant.UI;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulant.Core
{
    public sealed class PluginHost
    {
        public static PluginHost Instance { get; set; } // 仅为了调试方便

        private readonly Label _statusLabel;
        private readonly SimulantUI _ui;

        private readonly object _initLock = new object();

        internal PluginLog PluginLog = new PluginLog();

        internal bool CsvReady { get; private set; }
        internal bool CsvLoading { get; private set; }
        internal bool PluginReady { get; private set; }
        internal bool IsInitializing { get; private set; }
        internal bool FirewallEnabled => FirewallService != null && FirewallService.IsEnabled;

        internal Firewall.FirewallService FirewallService;
        internal ZoneService ZoneService;
        internal Environment.EnvironmentService EnvironmentService;
        internal EntityProvider EntityProvider;
        public PluginHost(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            _statusLabel = pluginStatusText;
            pluginScreenSpace.Text = "仿生石";

            _ui = new SimulantUI(this) { Dock = DockStyle.Fill };
            pluginScreenSpace.Controls.Add(_ui);

            FirewallService = new Firewall.FirewallService(this);
            ZoneService = new ZoneService(this);
            EnvironmentService = new Environment.EnvironmentService(this);
            EntityProvider = new EntityProvider(this);

            Attach();

            _statusLabel.Text = "仿生石已启动。";

            _ = LoadCsvAsync();
        }

        internal async Task LoadCsvAsync()
        {
            if (CsvReady || CsvLoading)
                return;

            CsvLoading = true;
            _ui?.UpdateControlStates();

            try
            {
                LogRuntime("正在加载 CSV 数据……");

                await Task.Run(() =>
                {
                    CsvManager.Instance.LoadAllTables();
                });

                CsvReady = true;
                LogRuntime("CSV 数据加载完成。");
            }
            catch (Exception ex)
            {
                CsvReady = false;
                LogError("CSV 数据加载失败：" + ex);
            }
            finally
            {
                CsvLoading = false;
                _ui?.UpdateControlStates();
            }
        }

        internal async Task InitAsync()
        {
            lock (_initLock)
            {
                if (IsInitializing)
                {
                    LogWarning("插件正在初始化，跳过重复请求。");
                    return;
                }

                IsInitializing = true;
            }

            _ui?.UpdateControlStates();

            try
            {
                LogRuntime("正在绑定鲶鱼精邮差……");
                await Task.Run(() => NamazuInterop.Init());

                LogRuntime("正在绑定 Triggernometry……");
                await Task.Run(() => TriggernometryInterop.Init());

                LogRuntime("正在扫描地址……");
                var scanResult = await Task.Run(() => SigAddressScanner.Scan());

                var errorLines = scanResult
                    .Where(x => !string.IsNullOrEmpty(x.Value))
                    .Select(x => $"{x.Key}: {x.Value}")
                    .ToList();

                foreach (var line in errorLines)
                {
                    LogError("地址扫描失败：" + line);
                }

                LogRuntime($"地址扫描完成，成功：{scanResult.Count - errorLines.Count} / {scanResult.Count}");

                PluginReady = true;
                LogRuntime("插件初始化完成。");
            }
            catch (Exception ex)
            {
                PluginReady = false;
                LogError("初始化插件失败：" + ex);
            }
            finally
            {
                lock (_initLock)
                {
                    IsInitializing = false;
                }

                _ui?.UpdateControlStates();
            }
        }

        public void Dispose()
        {
            try
            {
                if (FirewallService != null && FirewallService.IsEnabled)
                {
                    FirewallService.Disable();
                }
            }
            catch (Exception ex)
            {
                LogError("卸载时关闭防火墙失败：" + ex.Message);
            }

            if (XivPluginInterop.plugin != null)
            {
                XivPluginInterop.plugin.DataSubscription.ProcessChanged -= OnFFXIVProcessChanged;
            }

            _statusLabel.Text = "插件已卸载";
        }

        private void Attach()
        {
            lock (this)
            {
                if (ActGlobals.oFormActMain == null)
                {
                    XivPluginInterop.plugin = null;
                    return;
                }

                if (XivPluginInterop.plugin != null)
                    return;

                var ffxivPluginData = ActGlobals.oFormActMain.ActPlugins
                    .FirstOrDefault(x => x.pluginObj != null
                        && x.pluginObj.GetType().ToString() == "FFXIV_ACT_Plugin.FFXIV_ACT_Plugin");

                XivPluginInterop.plugin =
                    (FFXIV_ACT_Plugin.FFXIV_ACT_Plugin)ffxivPluginData?.pluginObj;

                if (XivPluginInterop.plugin == null)
                    return;

                Task.Run(() =>
                {
                    var isFFXIVPluginStarted = false;

                    while (!isFFXIVPluginStarted)
                    {
                        if (ffxivPluginData.lblPluginStatus.Text.ToUpper().Contains("STARTED"))
                        {
                            XivPluginInterop.plugin.DataSubscription.ProcessChanged += OnFFXIVProcessChanged;
                            OnFFXIVProcessChanged(XivPluginInterop.plugin.DataRepository.GetCurrentFFXIVProcess());
                            isFFXIVPluginStarted = true;
                            return;
                        }

                        Thread.Sleep(3000);
                    }
                });
            }
        }

        private void OnFFXIVProcessChanged(Process process)
        {
            var gameProcess = process;
            if (gameProcess == null)
                return;

            // var gameRegion = XivPluginInterop.GetRegion(XivPluginInterop.plugin);

            // 这里先只确认能拿到进程、版本、区服
            // 后面再接签名扫描、能力探测、运行时初始化
        }

        internal void Log(LogType type, string message)
        {
            PluginLog.Add(type, message);
            _ui?.RefreshLogView();
        }

        internal void LogError(string message) => Log(LogType.Error, message);
        internal void LogWarning(string message) => Log(LogType.Warning, message);
        internal void LogRuntime(string message) => Log(LogType.Runtime, message);
        internal void LogSim(string message) => Log(LogType.Sim, message);
        internal void LogVerbose(string message) => Log(LogType.Verbose, message);
    }
}