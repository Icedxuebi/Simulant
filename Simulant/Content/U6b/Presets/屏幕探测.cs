using Simulant.Content.U6b;
using Simulant.Core;
using Simulant.Core.Environment;
using System;

namespace Simulant.Presets
{
    internal class 屏幕探测 : SimPresetBase
    {
        public override int TerritoryId { get; } = 1122;
        public override string Name { get; } = "P3 屏幕探测";
        public override string Author { get; } = "Simulant";
        public override DateTime LastUpdated { get; } = new DateTime(2026, 4, 21);
        public override PhaseData Phase { get; } = U6b.P3;
        public override Type SimLogicType { get; } = typeof(U6b3_屏幕探测Logic);
        public override string Description { get; } = "占位符";
    }

    internal class U6b3_屏幕探测Logic : SimLogicBase
    {
        public override void Start()
        {
            throw new NotImplementedException();
        }
    }
}
