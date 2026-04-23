using Simulant.Game.ExtractedCsv.Rows;

namespace Simulant.Core.Zone
{
    internal sealed class ZoneSelectionState
    {
        public SimPresetBase Preset { get; private set; }
        public int TerritoryId { get; private set; }
        public TerritoryType Territory { get; private set; }
        public bool HasSelection { get; private set; }

        public void Set(SimPresetBase preset, int territoryId, TerritoryType territory)
        {
            Preset = preset;
            TerritoryId = territoryId;
            Territory = territory;
            HasSelection = true;
        }

        public void Clear()
        {
            Preset = null;
            TerritoryId = 0;
            Territory = null;
            HasSelection = false;
        }
    }
}