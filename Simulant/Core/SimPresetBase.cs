using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulant.Core
{
    public abstract class SimPresetBase
    {
        public abstract int TerritoryId { get; }
        public abstract string Name { get; }
        public abstract string Author { get; }
        public abstract DateTime LastUpdated { get; }
        public abstract Type SimLogicType { get; }
        public abstract string Description { get; }

        public SimLogicBase CreateSimLogic()
        {
            // TODO: Add error handling for invalid SimLogicType
            return SimLogicType == null ? null : (SimLogicBase)Activator.CreateInstance(SimLogicType);
        }
    }
}
