using Simulant.ACT;
using Simulant.Core;
using Simulant.Core.Environment;
using Simulant.Game.FFCS.Client.Game.Object;
using Simulant.Simulation;
using Simulant.Simulation.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using static System.Math;

namespace Simulant.Content.U7b.Presets
{
    internal class P5地火 : SimPresetBase
    {
        public override int TerritoryId { get; } = 1363;
        public override string Name { get; } = "P5地火";
        public override string Author { get; } = "阿洛";
        public override DateTime LastUpdated { get; } = new DateTime(2026, 6, 12);
        public override PhaseData Phase { get; } = U7b.P5;
        public override int Level => 100;
        public override Type SimLogicType { get; } = typeof(P5地火Logic);
        public override string Description { get; } = null;

        public override List<SimOptionBase> Options { get; } = new List<SimOptionBase>
        {
            new ComboBoxOption<ExaFlareOrder>(
                nameof(P5地火Logic.LeftExaFlareOrder),
                "左上方三轮地火出现顺序（视角面向左上）",
                ExaFlareOrder.Random,
                new Map<ExaFlareOrder>
                {
                    [ExaFlareOrder.Random] = "随机",
                    [ExaFlareOrder.Line14_25_36] = "1/4列 - 2/5列 - 3/6列",
                    [ExaFlareOrder.Line14_36_25] = "1/4列 - 3/6列 - 2/5列",
                    [ExaFlareOrder.Line25_14_36] = "2/5列 - 1/4列 - 3/6列",
                    [ExaFlareOrder.Line25_36_14] = "2/5列 - 3/6列 - 1/4列",
                    [ExaFlareOrder.Line36_14_25] = "3/6列 - 1/4列 - 2/5列",
                    [ExaFlareOrder.Line36_25_14] = "3/6列 - 2/5列 - 1/4列",
                }),

            new ComboBoxOption<ExaFlareOrder>(
                nameof(P5地火Logic.RightExaFlareOrder),
                "右上方三轮地火出现顺序（视角面向右上）",
                ExaFlareOrder.Random,
                new Map<ExaFlareOrder>
                {
                    [ExaFlareOrder.Random] = "随机",
                    [ExaFlareOrder.Line14_25_36] = "1/4列 - 2/5列 - 3/6列",
                    [ExaFlareOrder.Line14_36_25] = "1/4列 - 3/6列 - 2/5列",
                    [ExaFlareOrder.Line25_14_36] = "2/5列 - 1/4列 - 3/6列",
                    [ExaFlareOrder.Line25_36_14] = "2/5列 - 3/6列 - 1/4列",
                    [ExaFlareOrder.Line36_14_25] = "3/6列 - 1/4列 - 2/5列",
                    [ExaFlareOrder.Line36_25_14] = "3/6列 - 2/5列 - 1/4列",
                }),
        };
    }

    internal enum ExaFlareOrder
    {
        [RandomEnum(Line14_25_36, Line14_36_25, Line25_14_36, Line25_36_14, Line36_14_25, Line36_25_14)]
        Random,
        Line14_25_36, Line14_36_25, 
        Line25_14_36, Line25_36_14, 
        Line36_14_25, Line36_25_14
    }


    internal class P5地火Logic : SimLogicBase
    {
        [SimOption]
        internal ExaFlareOrder LeftExaFlareOrder { get; set; }
        
        [SimOption]
        internal ExaFlareOrder RightExaFlareOrder { get; set; }

        public P5地火Logic(PluginHost host, SimPresetBase preset) : base(host, preset)
        {
        }

        private bool _isPerfect;

        protected override void OnStart()
        {
            _isPerfect = true;
            Timeline().ContinueWith(t =>
            {
                if (t.Exception != null)
                    _host.LogError("模拟错误：" + t.Exception.GetBaseException());
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        protected override void OnStop()
        {
        }

        const float ExaFlareGroupDelay = 2.5f;

        private async Task Timeline()
        {
            var leftOrder = ResolveExaFlareOrder(LeftExaFlareOrder);
            var rightOrder = ResolveExaFlareOrder(RightExaFlareOrder);
            
            var boss = _host.EntitySpawner.SpawnBNpc(19511, 7131, 100); // Kefka
            boss.Pos3D = new Vector3(100, 100, 0);
            boss.Heading = (float)PI;
            boss.SetReadyToDraw();
            boss.EnableDraw();

            await Task.Delay(3000);
            using (var timer = new SimTimer(_host))
            {
                boss.Cast(0xBB3B); // 混沌末世 本体读条

                FireAndForget(SingleExaFlare(leftOrder[0], true));
                FireAndForget(SingleExaFlare(leftOrder[1], true));

                await timer.WaitUntil(ExaFlareGroupDelay * 1);

                FireAndForget(SingleExaFlare(rightOrder[0], false));
                FireAndForget(SingleExaFlare(rightOrder[1], false));

                await timer.WaitUntil(ExaFlareGroupDelay * 2);

                FireAndForget(SingleExaFlare(leftOrder[2], true));
                FireAndForget(SingleExaFlare(leftOrder[3], true));

                await timer.WaitUntil(ExaFlareGroupDelay * 3);

                FireAndForget(SingleExaFlare(rightOrder[2], false));
                FireAndForget(SingleExaFlare(rightOrder[3], false));

                await timer.WaitUntil(ExaFlareGroupDelay * 4);

                FireAndForget(SingleExaFlare(leftOrder[4], true));
                FireAndForget(SingleExaFlare(leftOrder[5], true));

                await timer.WaitUntil(ExaFlareGroupDelay * 5);

                FireAndForget(SingleExaFlare(rightOrder[4], false));
                FireAndForget(SingleExaFlare(rightOrder[5], false));

                await timer.WaitUntil(16.2);
                boss.Cast(0xBB3E); // 混沌末世 本体读条

                await timer.WaitUntil(22.0);
                var playerPositions = _host.EntityProvider.GetCharacters()
                    .Where(e => e.Native.ObjectKind == ObjectKind.Pc)
                    .Select(e => e.Pos3D)
                    .ToList();

                foreach (var pos in playerPositions)
                {
                    FireAndForget(DummyExecute(pos, 0f, 0xBB3F, 3f));
                }
                
                await timer.WaitUntil(23.5);
                if (_isPerfect)
                {
                    TriggernometryInterop.InvokeNamedCallback("LockOn", 
                        $"{_host.EntityProvider.GetMyself().Address}, m0489trg_a0c");
                }

                await timer.WaitUntil(27);
                _host.EntitySpawner.Delete(boss);
            }
        }

        private List<int> ResolveExaFlareOrder(ExaFlareOrder order)
        {
            switch (order)
            {
                case ExaFlareOrder.Line14_25_36:
                    return new List<int> { 1, 4, 2, 5, 3, 6 };
                case ExaFlareOrder.Line14_36_25:
                    return new List<int> { 1, 4, 3, 6, 2, 5 };
                case ExaFlareOrder.Line25_14_36:
                    return new List<int> { 2, 5, 1, 4, 3, 6 };
                case ExaFlareOrder.Line25_36_14:
                    return new List<int> { 2, 5, 3, 6, 1, 4 };
                case ExaFlareOrder.Line36_14_25:
                    return new List<int> { 3, 6, 1, 4, 2, 5 };
                case ExaFlareOrder.Line36_25_14:
                    return new List<int> { 3, 6, 2, 5, 1, 4 };
                default:
                    throw new ArgumentException("Invalid ExaFlareOrder: " + order);
            }
        }

        /// <param name="lineIdx"> 面向地火源头方向，从左至右记为 1-6。</param>
        /// <param name="isTopLeft"> 是左上方的地火，反之为右上方。</param>
        /// <returns></returns>
        private async Task SingleExaFlare(int lineIdx, bool isTopLeft)
        {
            var initPos = isTopLeft
                   ? new Vector3(100 - 35 + 5 * lineIdx, 100 - 5 * lineIdx, 0)
                   : new Vector3(100 + 5 * lineIdx, 100 - 35 + 5 * lineIdx, 0);

            var heading = (float)PI * (isTopLeft ? 1f : -1f) / 4f;

            var dPos = isTopLeft
                ? new Vector3(5, 5, 0)
                : new Vector3(-5, 5, 0);

            using (var timer = new SimTimer(_host))
            {
                // 后六次判定（不含初始，以 108 日志计）相对于初始预兆的线性拟合结果为：4.582, 5.095, 5.608, 6.121, 6.634, 7.147
                // 即第二次判定延迟 4.582 s，间隔 0.513 s
                var t0 = 4.582f;
                var dt = 0.513f;

                FireAndForget(DummyCast(initPos, heading, 0xBB3C, 6f));
                
                for (int i = 0; i < 6; i++)
                {
                    await timer.WaitUntil(t0 + dt * i);
                    ExecuteAndValidateExaFlare(initPos + dPos * (i + 1), heading);
                }
            }
        }

        private void ExecuteAndValidateExaFlare(Vector3 pos, float heading)
        {
            FireAndForget(DummyExecute(pos, heading, 0xBB3D, 3f));
            var player = _host.EntityProvider.GetMyself();
            if (Vector2.Distance(new Vector2(pos.X, pos.Y), player.Pos) <= 6f)
            {
                _isPerfect = false;
                TriggernometryInterop.InvokeNamedCallback("LockOn", $"{player.Address}, m0489trg_b0c");
            }
        }

    }

}