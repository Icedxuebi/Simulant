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
            _host = new PluginHost();
            _host.Initialize(pluginScreenSpace, pluginStatusText);
        }

        public void DeInitPlugin()
        {
            if (_host != null)
            {
                _host.Dispose();
                _host = null;
            }
        }
    }
}