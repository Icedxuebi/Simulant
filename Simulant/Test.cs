using Simulant.Core;
using Simulant.Core.Entity;
using Simulant.Core.Environment;
using Simulant.Game;
using Simulant.Game.FFCS.Client.Game.Event;
using Simulant.Game.FFCS.Client.System.Framework;
using System.Numerics;
using System.Threading.Tasks;

namespace Simulant
{
    internal class Test
    {
        private readonly PluginHost _host;
        internal Test(PluginHost host)
        {
            _host = host;
        }

        internal async Task Run()
        {
            TestEnvManager();
        }

        private void TestEnvManager()
        {
            var phaseData = Content.U6b.U6b.P3;
            _host.EnvironmentService.SetWeather(phaseData.Weather);
            _host.EnvironmentService.SetBgm(phaseData.BGM);

            for (uint slot = 0; slot < phaseData.MapEffectFlags.Count; slot++)
            {
                var flag = phaseData.MapEffectFlags[(int)slot];
                _host.EnvironmentService.PlayMapEffect(slot, flag);
            }
        }

        private void TestInstanceDirectors()
        {
            var ef = EventFramework.Instance;
            _host.LogRuntime($"EventFramework: {(ulong)ef.Ptr:X}");
            _host.LogRuntime($"ContentDirector: {(ulong)ef.ContentDirector.Ptr:X}");
            _host.LogRuntime($"GetContentDirector(): {(ulong)ef.GetContentDirector().Ptr:X}");
            _host.LogRuntime($"GetInstanceContentDirector(): {(ulong)ef.GetInstanceContentDirector().Ptr:X}");
            _host.LogRuntime($"GetPublicContentDirector(): {(ulong)ef.GetPublicContentDirector().Ptr:X}");
        }

        private async void EntityTimelineTest()
        {
            var me = _host.EntityProvider.GetMyself();

            var spawner = new EntitySpawner(_host, 100);
            var bnpc = spawner.SpawnBNpc(4909, 3765);

            bnpc.Pos3D = me.Pos3D + new Vector3(1, 0, 0);
            bnpc.Heading = me.Heading;
            bnpc.SetReadyToDraw();
            bnpc.EnableDraw();

            var intervalMs = 10;
            var speed = 6f;
            var durationMs = 7000;

            bnpc.PlayBaseTimeline(13);

            var elapsed = 0;
            while (elapsed < durationMs)
            {
                bnpc.Y += speed * (intervalMs / 1000f);
                bnpc.Native.DrawObject.Position.Set(bnpc.Native.Position);
                await Task.Delay(intervalMs);
                elapsed += intervalMs;
            }
            bnpc.PlayBlendTimeline(3201);
            await Task.Delay(3000);

            spawner.Delete(bnpc);
        }


        private void GetOnReceivePtr()
        {
            var ptr = Framework.Instance.NetworkModuleProxy.NetworkModule.PacketReceiverCallback.PacketDispatcher.Ptr.GetVFuncPtr(1);
            _host.LogRuntime($"OnReceive: {(ulong)ptr:X}");
        }
    }
}
