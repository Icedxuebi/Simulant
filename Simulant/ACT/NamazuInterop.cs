using System;
using System.Linq;
using Advanced_Combat_Tracker;

namespace Simulant.ACT
{
    internal sealed class NamazuInterop
    {
        public object RawPlugin { get; private set; }
        public NamazuPlugin Plugin { get; private set; }

        public bool IsReady
        {
            get { return Plugin != null && Plugin.IsReady; }
        }

        public void Initialize()
        {
            RawPlugin = null;
            Plugin = null;

            if (ActGlobals.oFormActMain == null)
                throw new Exception($"[{nameof(NamazuInterop)}] 未找到 ActGlobals.oFormActMain。");

            var pluginData = ActGlobals.oFormActMain.ActPlugins
                .FirstOrDefault(x =>
                    x.pluginObj != null &&
                    x.pluginObj.GetType().ToString() == "PostNamazu.PostNamazu")
                ?? throw new Exception($"[{nameof(NamazuInterop)}] 未找到 PostNamazu 插件，或插件未启用。");

            RawPlugin = pluginData.pluginObj;
            Plugin = new NamazuPlugin(RawPlugin);
        }
    }
}