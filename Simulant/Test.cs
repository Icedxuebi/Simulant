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
            NoFrameLockTest();
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
                foreach (var bnpc in spawned)
                {
                    try
                    {
                        spawner.Delete(bnpc);
                    }
                    catch
                    {
                    }
                }
            }
        }

        private async void FrameLockTest()
        {
            var me = _host.EntityProvider.GetMyself();

            var spawner = new EntitySpawner(_host, 100);
            var spawned = new List<dynamic>();

            await Task.Delay(3000);

            try
            {
                using (NamazuInterop.Plugin.Memory.AcquireFrame(true))
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

                }
                await Task.Delay(3000);
            }
            finally
            {
                foreach (var bnpc in spawned)
                {
                    try
                    {
                        spawner.Delete(bnpc);
                    }
                    catch
                    {
                    }
                }
            }
        }

        private async Task 宇宙天箭Test()
        { 
            
            var spawner = new EntitySpawner(_host, 90);

            Character Dummy(Vector3 pos, float heading)
            {
                var dummy = spawner.SpawnBNpc(9020);
                dummy.Pos3D = pos;
                dummy.Heading = heading;
                dummy.EnableDraw();
                dummy.Native.TargetableStatus.Set(0);
                return dummy;
            }

            async void Cast宽直条(Vector3 pos, float heading)
            {
                var dummy1 = Dummy(pos, heading);
                var dummy2 = Dummy(pos, heading);
                dummy1.Cast(0x7BA3); // omen only?
                dummy2.Cast(0x7E51); // arrows vfx only?
                await Task.Delay(10000);
                spawner.Delete(dummy1);
                spawner.Delete(dummy2);
            }

            async void Execute窄直条(Vector3 pos, float heading)
            {
                using (var _timer = new SimTimer(_host))
                {
                    var dummy = spawner.SpawnBNpc(9020);
                    dummy.Pos3D = pos;
                    dummy.Heading = heading;
                    dummy.Native.TargetableStatus.Set(0);
                    dummy.EnableDraw();
                    await _timer.WaitUntil(1);
                    dummy.Execute(0x7BA4);
                    await _timer.WaitUntil(4);
                    spawner.Delete(dummy);
                }
            }

            var boss = spawner.SpawnBNpc(15725, 12256); // alpha omega
            boss.Pos3D = new Vector3(100, 100, 0);
            boss.Heading = (float)Math.PI;
            boss.SetReadyToDraw();
            boss.EnableDraw();

            await Task.Delay(2000);

            // 时间轴开始
            var timer = new SimTimer(_host);
            boss.Cast(0x7BA2); // 射手天箭 本体读条
            Cast宽直条(new Vector3(80, 100, 0), (float)Math.PI / 2);
            Cast宽直条(new Vector3(100, 80, 0), 0f);

            await timer.WaitUntil(2);
            Cast宽直条(new Vector3(80, 85, 0), (float)Math.PI / 2);
            Cast宽直条(new Vector3(80, 115, 0), (float)Math.PI / 2);
            Cast宽直条(new Vector3(85, 80, 0), 0f);
            Cast宽直条(new Vector3(115, 80, 0), 0f);

            var Δt = 2.005;
            var Δpos = 5;

            var t0 = 10.6;
            for (int i = 0; i < 3; i++)
            {
                var idx = i; // 防止闭包捕获
                var t = t0 + idx * Δt - 1; // 提前一秒防止函数调用延迟
                timer.Schedule(t, () =>
                {
                    Execute窄直条(new Vector3(80, 107.5f + Δpos * idx, 0), (float)Math.PI / 2);
                    Execute窄直条(new Vector3(80,  92.5f - Δpos * idx, 0), (float)Math.PI / 2);
                    Execute窄直条(new Vector3(107.5f + Δpos * idx, 80, 0), 0f);
                    Execute窄直条(new Vector3(92.5f  - Δpos * idx, 80, 0), 0f);
                });
            }

            t0 = 12.6;
            for (int i = 0; i < 5; i++)
            {
                var idx = i;
                var t = t0 + idx * Δt - 1;
                timer.Schedule(t, () =>
                {
                    Execute窄直条(new Vector3(80, 107.5f - Δpos * idx, 0), (float)Math.PI / 2);
                    Execute窄直条(new Vector3(80, 92.5f + Δpos * idx, 0), (float)Math.PI / 2);
                    Execute窄直条(new Vector3(107.5f - Δpos * idx, 80, 0), 0f);
                    Execute窄直条(new Vector3(92.5f + Δpos * idx, 80, 0), 0f);
                });
            }

            await timer.WaitUntil(25);
            spawner.Delete(boss);
            timer.Dispose();
        }

        private async void EntityCastTest()
        {
            var me = _host.EntityProvider.GetMyself();

            var spawner = new EntitySpawner(_host, 90);
            var bnpc = spawner.SpawnBNpc(15717, 7636);

            bnpc.Pos3D = me.Pos3D;
            bnpc.Heading = me.Heading;
            bnpc.SetReadyToDraw();
            bnpc.EnableDraw();

            await Task.Delay(1000);
            bnpc.Cast(0x7B6B); // 探测式波动炮

            await Task.Delay(15000);
            spawner.Delete(bnpc);
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
