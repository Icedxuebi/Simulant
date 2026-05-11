using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulant.Content.U6b;
using Simulant.Core;
using Simulant.Core.Environment;
using Simulant.Simulation;

namespace Simulant.Presets
{
    internal class 宇宙天箭 : SimPresetBase
    {
        public override int TerritoryId { get; } = 1122;
        public override string Name { get; } = "P6 宇宙天箭";
        public override string Author { get; } = "Simulant";
        public override DateTime LastUpdated { get; } = new DateTime(2026, 4, 21);
        public override PhaseData Phase { get; } = U6b.P6;
        public override Type SimLogicType { get; } = typeof(U6b6_宇宙天箭Logic);
        public override string Description { get; } = "占位符";
    }

    internal class U6b6_宇宙天箭Logic : SimLogicBase
    {
        public override void Start()
        {
            throw new NotImplementedException();
        }
    }
}
