using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulant.Core
{
    public abstract class SimPresetBase
    {
        public abstract MapType Kind { get; }

        public abstract string Name { get; }

        public abstract string Author { get; }

        public abstract int TerritoryId { get; }

        public abstract Func<SimLogicBase> CreateLogic { get; }

    }

    public enum MapType
    {
        Ultimate,
        Savage,
        Others,
        Custom
    }
}
