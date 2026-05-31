using Simulant.ACT;
using Simulant.Core;
using Simulant.Core.Entity;
using Simulant.Core.Environment;
using Simulant.Game;
using Simulant.Game.FFCS.Client.Game.Event;
using Simulant.Game.FFCS.Client.Game.Object;
using Simulant.Game.FFCS.Client.System.Framework;
using Simulant.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
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
            EntityCastTest();
        }

        public void LogObjectArrays()
        {
            var objects = GameObjectManager.Instance().Objects;
            LogGameObjects("IndexSorted", objects.IndexSorted);
            LogGameObjects("GameObjectIdSorted", objects.GameObjectIdSorted);
            LogGameObjects("EntityIdSorted", objects.EntityIdSorted);

            void LogGameObjects(string name, GameObjectPtrs ptrs)
            {
                var sb = new StringBuilder();
                sb.AppendLine().AppendLine("== " + name + " ==");

                int i = 0;
                foreach (var obj in ptrs.GameObjects)
                {
                    if (!obj.IsNull())
                        sb.Append('#').Append(i)
                            .Append(" @ ").Append(obj.Ptr.Hex())
                            .Append(", Id=").Append(((uint)obj.EntityId).ToString("X8"))
                            .Append(", Index=").Append((ushort)obj.ObjectIndex)
                            .Append(", Type=").Append((ObjectKind)obj.ObjectKind)
                            .AppendLine();
                    i++;
                }
                _host.LogVerbose(sb.ToString());
            }
        }

        private async void NoFrameLockTest()
        {
            var me = _host.EntityProvider.GetMyself();

            var spawner = new EntitySpawner(_host, 100);
            var spawned = new List<dynamic>();

            await Task.Delay(3000);

            try
            {
                var count = 20;
                var radius = 5f;
                var basePos = me.Pos3D;
                var heading = me.Heading;

                for (var i = 0; i < count; i++)
                {
                    var angle = Math.PI * 2.0 * i / count;
                    var offset = new Vector3(
                        (float)Math.Sin(angle) * radius,
                        (float)Math.Cos(angle) * radius,
                        0);

                    var bnpc = spawner.SpawnBNpc(4909, 3765);
                    spawned.Add(bnpc);

                    bnpc.Pos3D = basePos + offset;
                    bnpc.Heading = heading;
                    bnpc.SetReadyToDraw();
                    bnpc.EnableDraw();
                }
                await Task.Delay(3000);
            }
            finally
            {
                spawned.ForEach(bnpc => spawner.Delete(bnpc));
            }
        }

        EntitySpawner _spawner;
        private void EntityCastTest()
        {
            var me = _host.EntityProvider.GetMyself();

            _spawner = _spawner ?? new EntitySpawner(_host, 90);
            var bnpc = _spawner.SpawnBNpc(15725, 12256);

            bnpc.Pos3D = me.Pos3D;
            bnpc.Heading = me.Heading;
            bnpc.SetReadyToDraw();
            bnpc.EnableDraw();

            bnpc.Cast(0x7BA2); // 宇宙天箭

            _host.LogVerbose($"CastInfo @ {bnpc.Native.CastInfo.Ptr.Hex()}");
            _host.LogVerbose($"VFunc #81 Ret @ {bnpc.Native.Ptr.CallVFunc<IntPtr>(81).Hex()}");
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
