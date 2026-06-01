using Simulant.Core;
using Simulant.Core.Environment;
using Simulant.Game;
using Simulant.Simulation;
using Simulant.Simulation.Options;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using static System.Math;

namespace Simulant.Content.U7a.Presets
{
    internal class P5地火 : SimPresetBase
    {
        public override int TerritoryId { get; } = 1238;
        public override string Name { get; } = "P5 地火";
        public override string Author { get; } = "阿洛";
        public override DateTime LastUpdated { get; } = new DateTime(2026, 5, 12);
        public override PhaseData Phase { get; } = U7a.P5;
        public override int Level => 100;
        public override Type SimLogicType { get; } = typeof(P5地火Logic);
        public override string Description { get; } = null;

        public override List<SimOptionBase> Options { get; } = new List<SimOptionBase>
        {
            new ComboBoxOption<CenterDir4>(
                nameof(P5地火Logic.CenterDir4),
                "地火安全点相对于中心的方向",
                CenterDir4.Random,
                new Map<CenterDir4>
                {
                    [CenterDir4.Random] = "随机",
                    [CenterDir4.N] = "北 (0)",
                    [CenterDir4.W] = "西 (1)",
                    [CenterDir4.S] = "南 (2)",
                    [CenterDir4.E] = "东 (3)",
                }),

            new ComboBoxOption<FirstLineDir8>(
                nameof(P5地火Logic.FirstLineDir8),
                "地火第一根线相对于安全点的方向",
                FirstLineDir8.Random,
                new Map<FirstLineDir8>
                {
                    [FirstLineDir8.Random] = "随机",
                    [FirstLineDir8.N] = "北 (0)",
                    [FirstLineDir8.NW] = "西北 (1)",
                    [FirstLineDir8.W] = "西 (2)",
                    [FirstLineDir8.SW] = "西南 (3)",
                    [FirstLineDir8.S] = "南 (4)",
                    [FirstLineDir8.SE] = "东南 (5)",
                    [FirstLineDir8.E] = "东 (6)",
                    [FirstLineDir8.NE] = "东北 (7)",
                }),

            new ComboBoxOption<RotateDirection>(
                nameof(P5地火Logic.RotateDirection),
                "地火旋转方向",
                RotateDirection.Random,
                new Map<RotateDirection>
                {
                    [RotateDirection.Random] = "随机",
                    [RotateDirection.ClockWise] = "顺时针",
                    [RotateDirection.CounterClockwise] = "逆时针",
                }),
        };

        public P5地火()
        {
        }

    }

    internal enum CenterDir4
    {
        [RandomEnum(N, W, S, E)]
        Random = -1,
        N = 0, W = 1, S = 2, E = 3
    }

    internal enum FirstLineDir8
    {
        [RandomEnum(N, NW, W, SW, S, SE, E, NE)]
        Random = -1,
        N = 0, NW = 1, W = 2, SW = 3, S = 4, SE = 5, E = 6, NE = 7
    }

    internal enum RotateDirection
    {
        [RandomEnum(ClockWise, CounterClockwise)]
        Random,
        ClockWise,
        CounterClockwise
    }

    internal class P5地火Logic : SimLogicBase
    {
        [SimOption]
        internal CenterDir4 CenterDir4 { get; set; }

        [SimOption]
        internal FirstLineDir8 FirstLineDir8 { get; set; }

        [SimOption]
        internal RotateDirection RotateDirection { get; set; }

        public P5地火Logic(PluginHost host, SimPresetBase preset) : base(host, preset)
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
            var boss = _host.EntitySpawner.SpawnBNpc(17839, 13561, 100); // Pandora
            boss.Pos3D = new Vector3(100, 100, 0);
            boss.Heading = (float)PI;
            boss.SetReadyToDraw();
            boss.EnableDraw();

            await Task.Delay(3000);

            // 时间轴开始，读条的同时生成 EObj，但不播放动画。
            var timer = new SimTimer(_host);
            boss.Cast(0x9D72); // 地火 本体读条

            var lines = new List<Simulant.Core.Entity.EventObject>();
            var eObjsPose = CalculateEObjsPose();
            for (int i = 0; i < 6; i++)
            {
                var (pos, heading) = eObjsPose[i];
                var eObj = _host.EntitySpawner.SpawnEObj((ushort)i, 2014199, pos, heading); // no animation, timeline state 0
                lines.Add(eObj);
            }

            await timer.WaitUntil(8);
            lines.ForEach(e => e.PlayAnimation(0x2)); // 8s 时所有线同时出现

            await timer.WaitUntil(10);
            FireAndForget(SingleTimelineStartFromGlowing(lines[0]));
            FireAndForget(SingleTimelineStartFromGlowing(lines[1]));

            await timer.WaitUntil(14);
            FireAndForget(SingleTimelineStartFromGlowing(lines[2]));
            FireAndForget(SingleTimelineStartFromGlowing(lines[3]));

            await timer.WaitUntil(18);
            FireAndForget(SingleTimelineStartFromGlowing(lines[4]));
            FireAndForget(SingleTimelineStartFromGlowing(lines[5]));

            await timer.WaitUntil(20);
            boss.Heading = (float)((int)CenterDir4 / 2.0 * PI - PI);

            await timer.WaitUntil(25);
            boss.Cast(0x9D76); // 双分摊

            await timer.WaitUntil(42);
            _host.EntitySpawner.Delete(boss);
            timer.Dispose();
        }

        /// <summary> 每条地火直线从细线扩大开始的完整时间轴。 </summary>
        private async Task SingleTimelineStartFromGlowing(Simulant.Core.Entity.EventObject lineEObj)
        {
            // 实体面向与直线垂直，面向光侧

            _host.LogVerbose($"地火线 {lineEObj.Id} @ {lineEObj.Native.Ptr.Hex()} 开始时间轴: Pos = <{lineEObj.Pos.X:F2}, {lineEObj.Pos.Y:F2}>, Heading = {lineEObj.Heading:F2}");
            var timer = new SimTimer(_host);
            var pos = lineEObj.Pos3D;
            var heading = lineEObj.Heading;
            var oppositeHeading = (float)(lineEObj.Heading + PI);

            lineEObj.PlayAnimation(0x10);
            // 初始的矩形 Omen，技能实际没有其他效果
            FireAndForget(DummyCast(pos, heading, 0x9D73, 10, 5)); // 光
            FireAndForget(DummyCast(pos, oppositeHeading, 0x9CB6, 10, 5)); // 暗

            await timer.WaitUntil(7);
            lineEObj.PlayAnimation(0x8); // 箭头特效

            FireAndForget(QueueSteps(pos, heading, isLight: true));
            FireAndForget(QueueSteps(pos, oppositeHeading, isLight: false));

            timer.Dispose();
        }

        private async Task QueueSteps(Vector3 pos, float heading, bool isLight)
        {
            var timer = new SimTimer(_host);
            var step = new Vector3((float)(5 * Sin(heading)), (float)(5 * Cos(heading)), 0);
            uint abilityId = isLight ? 0x9D74u : 0x9D75u;
            var center = new Vector3(100, 100, 0);

            for (int i = 1; i < 10; i++)
            {
                await timer.WaitUntil(2 * i);
                var currentPos = pos + step * i;
                if (Vector3.Distance(currentPos, center) > 21)
                    break;

                FireAndForget(DummyExecute(currentPos, heading, abilityId, 3));
            }
            timer.Dispose();
        }

        /// <summary>
        /// 计算绝伊甸地火八角星的 6 条边的地火实体相对于场中心的位姿。 
        /// </summary>
        /// <returns>
        /// 返回一个长度为 6 的数组。<br />
        /// 每个元素表示一条边的中心点及其方向角度。<br />
        /// 发光顺序为 01 - 23 - 45，间隔 4 秒。  
        /// </returns>
        (Vector3 pos, float heading)[] CalculateEObjsPose()
        {
            int centerDir4 = (int)CenterDir4;
            bool isClockWise = RotateDirection == RotateDirection.ClockWise;
            int firstLineDir8 = (int)FirstLineDir8;

            // 八角星中心方向（0~3）
            double theta4 = centerDir4 * (PI / 2) - PI;
            Vector2 octagonCenter = 7.0711f * new Vector2((float)Sin(theta4), (float)Cos(theta4)); // 八角星中心距离场地中心 5√2

            var result = new (Vector3 pos, float heading)[6];
            for (int i = 0; i < 6; i++)
            {
                // 当前方向（未取余至 0-7）
                int eObjDir8Raw = firstLineDir8 + (isClockWise ? -i : i);
                double eObjHeading = eObjDir8Raw * (PI / 4) - PI;

                var sin = (float)Sin(eObjHeading);
                var cos = (float)Cos(eObjHeading);

                // 八角星每条边的中心点
                Vector2 pos = octagonCenter + 10f * new Vector2(sin, cos);

                // 平移至场地中心到八角星此边直线垂足位置
                // 垂线方向单位向量 u
                var unit = new Vector2(cos, -sin);

                // 平移距离 k 至 (x+ku, y+ku), 使得 (x+ku) / (y+ku) = sinθ / cosθ, 求解 k
                var k = pos.Y * sin - pos.X * cos;
                pos += k * unit;

                // 令相邻两条线正对中心方向光暗交替
                if (eObjDir8Raw % 2 == 0)
                    eObjHeading += PI;

                var absolutePos = new Vector3(pos.X + 100, pos.Y + 100, 0);
                result[i] = (absolutePos, (float)eObjHeading);

                _host.LogVerbose($"地火线 {i} 计算结果：Pos = <{absolutePos.X:F2}, {absolutePos.Y:F2}>, heading = {result[i].heading:F2}");
            }

            return result;
        }


    }

}