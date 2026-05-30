using Simulant.Content.U6b;
using Simulant.Core;
using Simulant.Core.Entity;
using Simulant.Core.Environment;
using Simulant.Game.FFCS.Client.Game.Object;
using Simulant.Simulation;
using Simulant.Simulation.Options;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace Simulant.Content.U6b.Presets
{
    internal class 宇宙天箭 : SimPresetBase
    {
        public override int TerritoryId { get; } = 1122;
        public override string Name { get; } = "P6 宇宙天箭";
        public override string Author { get; } = "阿洛";
        public override DateTime LastUpdated { get; } = new DateTime(2026, 5, 12);
        public override PhaseData Phase { get; } = U6b.P6;
        public override int Level => 100;
        public override Type SimLogicType { get; } = typeof(U6b6_宇宙天箭Logic);
        public override string Description { get; } = "目前仅实现了宇宙天箭部分，并未实现分散/分摊后续机制。";

        public override List<SimOptionBase> Options { get; } = new List<SimOptionBase>
        {
            new ComboBoxOption<int>(
                nameof(U6b6_宇宙天箭Logic.MyPartyIndex), 
                "自身职能", 
                1,
                new Map<int> // to-do: Role8Option : ComboBoxOption, enum Role8，IsT(this Role8 role) ...
                {
                    [1] = "MT",
                    [2] = "ST",
                    [3] = "H1",
                    [4] = "H2",
                    [5] = "D1",
                    [6] = "D2",
                    [7] = "D3",
                    [8] = "D4",
                }),

            new ComboBoxOption<ArrowMode>(
                nameof(U6b6_宇宙天箭Logic.ArrowMode), 
                "宇宙天箭模式", 
                ArrowMode.Random,
                new Map<ArrowMode>
                {
                    [ArrowMode.Random] = "随机",
                    [ArrowMode.OutsideFirst] = "先外侧",
                    [ArrowMode.InsideFirst] = "先内侧",
                }),

            new ComboBoxOption<bool>(
                nameof(U6b6_宇宙天箭Logic.IsSecondRound), 
                "轮次", 
                false,
                new Map<bool>
                {
                    [false] = "第一轮（116 死刑 / 分摊）",
                    [true] = "第二轮（八方分散 + 挡枪）",
                }),
        };

        public 宇宙天箭()
        {
        }

    }

    internal enum ArrowMode
    {
        [RandomEnum(OutsideFirst, InsideFirst)]
        Random,
        OutsideFirst,
        InsideFirst,
    }

    internal class U6b6_宇宙天箭Logic : SimLogicBase
    {

        [SimOption]
        internal int MyPartyIndex { get; set; }

        [SimOption]
        internal ArrowMode ArrowMode { get; set; }

        [SimOption]
        internal bool IsSecondRound { get; set; }

        private EntitySpawner _spawner; // to-do: SimEntityManager 统一管理实体的生成和删除

        public U6b6_宇宙天箭Logic(PluginHost host, SimPresetBase preset) : base(host, preset)
        {
        }

        protected override void OnStart()
        {
            _spawner = new EntitySpawner(_host, 90);

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
            var boss = _spawner.SpawnBNpc(15725, 12256); // alpha omega
            boss.Pos3D = new Vector3(100, 100, 0);
            boss.Heading = (float)Math.PI;
            boss.SetReadyToDraw();
            boss.EnableDraw();

            await Task.Delay(3000);

            // 时间轴开始
            var timer = new SimTimer(_host);
            boss.Cast(0x7BA2); // 射手天箭 本体读条
            if (ArrowMode == ArrowMode.OutsideFirst)
            {
                OutsideArrowTimeline();
                await timer.WaitUntil(2);
                InsideArrowTimeline();
            }
            else
            {
                InsideArrowTimeline();
                await timer.WaitUntil(2);
                OutsideArrowTimeline();
            }

            await timer.WaitUntil(25);
            _spawner.Delete(boss);
            timer.Dispose();
        }

        private void InsideArrowTimeline()
        {
            FireAndForget(SingleArrowTimeline(new Vector3(80, 100, 0), (float)Math.PI / 2));
            FireAndForget(SingleArrowTimeline(new Vector3(100, 80, 0), 0f));
        }

        private void OutsideArrowTimeline()
        {
            FireAndForget(SingleArrowTimeline(new Vector3(80, 85, 0), (float)Math.PI / 2));
            FireAndForget(SingleArrowTimeline(new Vector3(80, 115, 0), (float)Math.PI / 2));
            FireAndForget(SingleArrowTimeline(new Vector3(85, 80, 0), 0f));
            FireAndForget(SingleArrowTimeline(new Vector3(115, 80, 0), 0f));
        }

        /// <summary> 每个初始宇宙天箭宽读条及后续扩散窄直条的完整时间轴。 </summary>
        private async Task SingleArrowTimeline(Vector3 pos, float heading)
        {
            var timer = new SimTimer(_host);

            var Δt = 2.005;
            Vector3 dir = new Vector3(
                (float)Math.Cos(heading), 
                (float)Math.Sin(heading), 
                0);
            
            var t0 = 10.6;

            // 初始宽直条
            WideArrow(pos, heading);

            for (int step = 0; step < 10; step++)
            {
                await timer.WaitUntil(t0 + Δt * step);

                var offset = 7.5f + 5f * step;
                var pos1 = pos + dir * offset;
                var pos2 = pos - dir * offset;

                var valid1 = TryThinArrow(pos1, heading);
                var valid2 = TryThinArrow(pos2, heading);

                if (!valid1 && !valid2) break;
            }

            await Task.Delay(3000);
            timer.Dispose();
        }

        Character Dummy(Vector3 pos, float heading)
        {
            var dummy = _spawner.SpawnBNpc(9020);
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
            _spawner.Delete(dummy);
        }

        async Task DummyExecute(Vector3 pos, float heading, uint abilityId, float despawnSeconds)
        {
            var dummy = Dummy(pos, heading);
            dummy.Execute(abilityId);
            await Task.Delay(TimeSpan.FromSeconds(despawnSeconds));
            _spawner.Delete(dummy);
        }

        private void WideArrow(Vector3 pos, float heading)
        {
            FireAndForget(DummyCast(pos, heading, 0x7BA3, 10f)); // omen
            FireAndForget(DummyCast(pos, heading, 0x7E51, 10f)); // arrows vfx
        }

        private bool TryThinArrow(Vector3 pos, float heading)
        {
            if (Math.Abs(pos.X - 100) > 21 || Math.Abs(pos.Y - 100) > 21)
                return false; // 已经出了场地范围

            FireAndForget(DummyExecute(pos, heading, 0x7BA4, 3f));
            return true;
        }

        private void FireAndForget(Task task)
        {
            if (task == null) return;

            task.ContinueWith(
                t => _host.LogError("模拟错误：" + t.Exception.GetBaseException()),
                TaskContinuationOptions.OnlyOnFaulted
            );
        }

    }
}
