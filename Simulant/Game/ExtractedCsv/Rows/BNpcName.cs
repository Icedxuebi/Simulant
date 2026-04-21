using System;
using System.Collections.Generic;

namespace Simulant.Game.ExtractedCsv.Rows
{
    public class BNpcName : TypedCsvRow
    {
        public override string Name => Get(1);
    }

}