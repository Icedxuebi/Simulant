using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulant.Core;

namespace Simulant.Presets
{
    internal class U6b6_宇宙天箭 : SimPresetBase
    {
        public override MapType Kind => MapType.Ultimate;
        public override int TerritoryId => 1122;
        public override string Name => "P6 宇宙天箭";
        public override string Author => "Simulant";
        public override Func<SimLogicBase> CreateLogic => () => new U6b6_宇宙天箭Logic();
    }

    internal class U6b6_宇宙天箭Logic : SimLogicBase
    {
        public override void Start()
        {
            throw new NotImplementedException();
        }
    }
}
