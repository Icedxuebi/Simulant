using Advanced_Combat_Tracker;
using Simulant.ACT;
using Simulant.UI;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulant.Core
{
    internal sealed class PluginHost
    {
        private Label _statusLabel;
        private SimulantUI _ui;

        public void Initialize(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            _statusLabel = pluginStatusText;

            pluginScreenSpace.Text = "仿生石";

            _ui = new SimulantUI();
            _ui.Dock = DockStyle.Fill;
            pluginScreenSpace.Controls.Add(_ui);

            Attach();

            _statusLabel.Text = "初始化完成";
        }

        public void Dispose()
        {
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

                var waitingFFXIVPlugin = new Task(() =>
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

                waitingFFXIVPlugin.Start();
            }
        }

        private void OnFFXIVProcessChanged(Process process)
        {
            var gameProcess = process;
            if (gameProcess == null)
                return;

            var gameRegion = XivPluginInterop.GetRegion(XivPluginInterop.plugin);

            // 这里先只确认能拿到进程、版本、区服
            // 后面再接签名扫描、能力探测、运行时初始化
        }
    }
}