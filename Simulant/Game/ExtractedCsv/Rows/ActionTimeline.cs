using System;
using System.Collections.Generic;

namespace Simulant.Game.ExtractedCsv.Rows
{
    public class ActionTimeline : TypedCsvRow
    {
        // "battle/battle_start"
        public override string Name => Get("Key");
    }

}