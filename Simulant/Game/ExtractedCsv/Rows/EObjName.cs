using System;
using System.Collections.Generic;

namespace Simulant.Game.ExtractedCsv.Rows
{
    public class EObjName : TypedCsvRow
    {
        public override string Name => Get(1);

    }

}