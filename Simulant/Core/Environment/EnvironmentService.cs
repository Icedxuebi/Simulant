namespace Simulant.Core.Environment
{
    internal sealed class EnvironmentService
    {
        private readonly PluginHost _host;

        public EnvironmentService(PluginHost host)
        {
            _host = host;
        }

        public void SetWeather(byte weatherId)
        {
            _host.LogWarning($"SetWeather 尚未实现");
        }

        public void PlayMapEffect(int slot, ushort flag)
        {
            _host.LogWarning($"PlayMapEffect 尚未实现。");
        }

        public void SetBgm(uint bgmId)
        {
            _host.LogWarning($"SetBgm 尚未实现。");
        }
    }
}