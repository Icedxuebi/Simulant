using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulant.ACT
{
    internal static class XivPluginInterop
    {
        /// <summary>
        /// 解析插件实例
        /// </summary>
        internal static FFXIV_ACT_Plugin.FFXIV_ACT_Plugin plugin;

        /// <summary>
        /// 获取FFXIV解析插件的区域信息。
        /// </summary>
        /// <param name="ffxiv_plugin"></param>
        /// <returns></returns>
        public static string GetRegion(FFXIV_ACT_Plugin.FFXIV_ACT_Plugin ffxiv_plugin)
        {
            switch (GetLanguageId(ffxiv_plugin))
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    return "intl";
                case 5:
                    return "cn";
                case 6:
                    return "ko";
                default:
                    return null;
            }
        }

        /// <summary>
        /// 直接获取原始的FFXIV解析插件语言ID。
        /// </summary>
        /// <param name="ffxiv_plugin"></param>
        /// <returns></returns>
        public static int GetLanguageId(FFXIV_ACT_Plugin.FFXIV_ACT_Plugin ffxiv_plugin)
        {
            if (ffxiv_plugin == null)
            {
                if (plugin == null) return 0;
                ffxiv_plugin = plugin;
            }
            try
            {
                return (int)ffxiv_plugin.DataRepository.GetSelectedLanguageID();
            }
            catch
            {
                return 0;
            }
        }
    }
}
