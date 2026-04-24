using Simulant.Core;
using Simulant.Core.Entity;
using Simulant.Game.FFCS.Client.System.Framework;
using Simulant.Game.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            _host.LogSim($"OnReceive: {(ulong)ptr:X}");
        }
    }
}
