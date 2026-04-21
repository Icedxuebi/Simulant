using System;
using System.Collections.Generic;

namespace Simulant.Game.ExtractedCsv.Rows
{
    public class PlaceName : TypedCsvRow
    {
        public override string Name => Get("Name");
    }

}