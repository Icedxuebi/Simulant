using System;
using System.Linq;
using Advanced_Combat_Tracker;

namespace Simulant.ACT
{
    public static class NamazuInterop
    {
        public static object RawPlugin { get; private set; }
        public static NamazuPlugin Plugin { get; private set; }

        public static bool IsReady
        {
            get { return Plugin != null && Plugin.IsReady; }
        }

        public static void Init()
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

        public static void DeInit()
        {
            RawPlugin = null;
            Plugin = null;
        }
    }
}