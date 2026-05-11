using Simulant.Content.U6b;
using Simulant.Core;
using Simulant.Core.Environment;
using Simulant.Simulation;
using Simulant.Simulation.Options;
using System;

namespace Simulant.Presets
{
    internal class 宇宙天箭 : SimPresetBase
    {
        public override int TerritoryId { get; } = 1122;
        public override string Name { get; } = "P6 宇宙天箭";
        public override string Author { get; } = "阿洛";
        public override DateTime LastUpdated { get; } = new DateTime(2026, 5, 12);
        public override PhaseData Phase { get; } = U6b.P6;
        public override Type SimLogicType { get; } = typeof(U6b6_宇宙天箭Logic);
        public override string Description { get; } = null;

        internal int Role { get; set; }
        internal ArrowMode ArrowMode { get; set; }
        internal bool IsSecondRound { get; set; }

        public 宇宙天箭()
        {
            AddOption(new ComboBoxOption<int>( 
                nameof(Role), "自身职能", 1, 
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
                }));

            AddOption(new ComboBoxOption<ArrowMode>(
                nameof(ArrowMode), "宇宙天箭模式", ArrowMode.Random,
                new Map<ArrowMode>
                {
                    [ArrowMode.Random] = "随机",
                    [ArrowMode.OutsideFirst] = "先外侧",
                    [ArrowMode.InsideFirst] = "先内侧",
                }));

            AddOption(new ComboBoxOption<bool>(
                nameof(IsSecondRound), "轮次", false,
                new Map<bool>
                {
                    [false] = "第一轮（116 死刑 / 分摊）",
                    [true] = "第二轮（八方分散 + 挡枪）",
                }));
        }

    }

    internal enum ArrowMode
    {
        Random = 0,
        OutsideFirst = 1,
        InsideFirst = 2,
    }

    internal class U6b6_宇宙天箭Logic : SimLogicBase
    {
        public override void Start()
        {
            throw new NotImplementedException();
        }
    }
}
