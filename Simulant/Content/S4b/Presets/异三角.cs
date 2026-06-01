using Simulant.Core;
using Simulant.Core.Entity;
using Simulant.Core.Environment;
using Simulant.Game.FFCS.Client.Game.Object;
using Simulant.Simulation;
using Simulant.Simulation.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Simulant.Content.S4b.Presets
{
    internal class 异三角 : SimPresetBase
    {
        public override int TerritoryId { get; } = 755;
        public override string Name { get; } = "本体 异三角";
        public override string Author { get; } = "阿洛";
        public override DateTime LastUpdated { get; } = new DateTime(2026, 6, 1);
        public override PhaseData Phase { get; } = O8s.本体P1;
        public override int Level => 80;
        public override Type SimLogicType { get; } = typeof(O8s_异三角Logic);
        public override string Description { get; } = "新玩具";

        public override List<SimOptionBase> Options { get; } = new List<SimOptionBase>
        {
        };

    }

    internal class O8s_异三角Logic : SimLogicBase
    {
        public O8s_异三角Logic(PluginHost host, SimPresetBase preset) : base(host, preset)
        {
        }

        protected override void OnStart()
        {
            Timeline().ContinueWith(t =>
            {
                if (t.Exception != null)
                    _host.LogError("模拟错误：" + t.Exception.GetBaseException());
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        protected override void OnStop()
        {
        }

        private async Task Timeline()
        {
            _remainingTriangles = _candidateTriangles
                .Select((triangle, i) => (triangle, (byte)(10 + i)))
                .ToList();

            var boss = _host.EntitySpawner.SpawnBNpc(8528, 7131, 80);
            boss.Pos3D = new Vector3(0, 0, 0);
            boss.Heading = (float)Math.PI;
            boss.SetReadyToDraw();
            boss.EnableDraw();

            await Task.Delay(3000);

            // 时间轴开始    
            var timer = new SimTimer(_host);
            boss.Cast(0x290D); // 异三角 本体读条

            await timer.WaitUntil(3.2);
            FireAndForgetMultipleTriangles(2);

            await timer.WaitUntil(5.2);
            FireAndForgetMultipleTriangles(2);

            await timer.WaitUntil(7.2);
            FireAndForgetMultipleTriangles(3);

            await timer.WaitUntil(20);
            _host.EntitySpawner.Delete(boss);
            timer.Dispose();
        }

        private void FireAndForgetMultipleTriangles(int count)
        {
            for (int i = 0; i < count; i++)
            {
                FireAndForget(SingleTriangleTimeline());
            }
        }

        /// <summary> 每个初始宇宙天箭宽读条及后续扩散窄直条的完整时间轴。 </summary>
        private async Task SingleTriangleTimeline()
        {
            var timer = new SimTimer(_host);

            (Triangle triangle, byte index) = GetRandomTriangleFromList();
            var eobj = triangle.CreateEObj(_host, index);
            
            await timer.WaitUntil(1.1);
            eobj.PlayAnimation(0x20);

            await timer.WaitUntil(2.6);
            eobj.PlayAnimation(0x80);

            await timer.WaitUntil(9.6);
            eobj.PlayAnimation(0x8);

            await timer.WaitUntil(9.7);
            var abilityPositions = triangle.ResolveAbilityPos();
            foreach (var pos in abilityPositions)
            {
                FireAndForget(DummyExecute(pos, 0f, 0x290E, 3f)); // 实际爆炸的圆形 AoE
            }

            timer.Dispose();
        }

        Character Dummy(Vector3 pos, float heading)
        {
            var dummy = _host.EntitySpawner.SpawnBNpc(9020, 0, 100);
            dummy.Pos3D = pos;
            dummy.Heading = heading;
            // 不可见，但是可以绘制技能特效
            // Native.SetReadyToDraw 就是在设置这个 flag
            dummy.Native.TargetableStatus.Set(ObjectTargetableFlags.ReadyToDraw);
            dummy.EnableDraw();
            return dummy;
        }

        async Task DummyCast(Vector3 pos, float heading, uint abilityId, float despawnSeconds)
        {
            var dummy = Dummy(pos, heading);
            dummy.Cast(abilityId);
            await Task.Delay(TimeSpan.FromSeconds(despawnSeconds));
            _host.EntitySpawner.Delete(dummy);
        }

        async Task DummyExecute(Vector3 pos, float heading, uint abilityId, float despawnSeconds)
        {
            var dummy = Dummy(pos, heading);
            dummy.Execute(abilityId);
            await Task.Delay(TimeSpan.FromSeconds(despawnSeconds));
            _host.EntitySpawner.Delete(dummy);
        }

        private void FireAndForget(Task task)
        {
            if (task == null) return;

            task.ContinueWith(
                t => _host.LogError("模拟错误：" + t.Exception.GetBaseException()),
                TaskContinuationOptions.OnlyOnFaulted
            );
        }

        private List<Triangle> _candidateTriangles = new List<Triangle>
        {
            new Triangle(14.4338f, -5.0000f, TriangleDirection.Left),
            new Triangle(11.5470f, 10.0000f, TriangleDirection.Right),
            new Triangle(-11.5470f, -10.0000f, TriangleDirection.Left),
            new Triangle(-14.4338f, 5.0000f, TriangleDirection.Right),
            new Triangle(2.8868f, -15.0000f, TriangleDirection.Right),
            new Triangle(0.0000f, 0.0000f, TriangleDirection.Both),
            new Triangle(-2.8868f, 15.0000f, TriangleDirection.Left),
        };

        private readonly Random _random = new Random();
        private readonly object _randomLock = new object();
        private List<(Triangle Triangle, byte ObjectIndex)> _remainingTriangles;

        /// <summary>
        /// 从候选三角形列表中随机选择一个。
        /// </summary>
        /// <returns></returns>
        private (Triangle triangle, byte index) GetRandomTriangleFromList()
        {
            lock (_randomLock)
            {
                if (_remainingTriangles == null || _remainingTriangles.Count == 0)
                    throw new InvalidOperationException("没有可用的异三角候选位置。");

                int i = _random.Next(_remainingTriangles.Count);
                var item = _remainingTriangles[i];
                _remainingTriangles.RemoveAt(i);

                return (item.Triangle, item.ObjectIndex);
            }
        }

        public enum TriangleDirection
        {
            Left,  // 1E84B4
            Right, // 1E84B3
            [RandomEnum(Left, Right)]
            Both
        }

        public class Triangle
        {
            public Vector2 PosXY;
            public bool IsLeft;

            public Triangle(float x, float y, TriangleDirection direction)
            {
                PosXY = new Vector2(x, y);
                IsLeft = direction.TryResolveRandom() == TriangleDirection.Left;
            }

            public Simulant.Core.Entity.EventObject CreateEObj(PluginHost host, byte objectIndex)
            {
                var data = new EObjData
                {
                    Index = objectIndex,
                    BaseId = IsLeft ? 2000052u : 2000051u,
                    SharedTimelineState = 0x2,
                    Pos = new Vector3(PosXY.X, PosXY.Y, 0),
                };
                return host.EntitySpawner.SpawnEObj(data);
            }

            /// <summary>
            /// 计算三个实际释放小圆形技能的位置。
            /// </summary>
            public List<Vector3> ResolveAbilityPos()
            {
                var r = (IsLeft ? 1 : -1) * (float)(10.0 / Math.Sqrt(3.0));

                return new List<Vector3>
                {
                    new Vector3(PosXY.X - r, PosXY.Y, 0f),
                    new Vector3(PosXY.X + r / 2f, PosXY.Y - 5f, 0f),
                    new Vector3(PosXY.X + r / 2f, PosXY.Y + 5f, 0f),
                };
            }
        }

    }
}
