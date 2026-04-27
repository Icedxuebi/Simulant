using System.Windows.Forms;
using Advanced_Combat_Tracker;
using Simulant.Core;

namespace Simulant
{
    public class PluginMain : IActPluginV1
    {
        private PluginHost _host;

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            PluginHost.Instance = new PluginHost(pluginScreenSpace, pluginStatusText);
            _host = PluginHost.Instance;
        }

        public void DeInitPlugin()
        {
            PluginHost.Instance = null;
            if (_host != null)
            {
                _host.Dispose();
                _host = null;
            }
        }
    }
}