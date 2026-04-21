using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Simulant.Game.ExtractedCsv.Rows
{
    public class ActionCastVfx : TypedCsvRow
    {
        public override string Name => Vfx.Name;

        public ushort VfxId => Get<ushort>("VFX"); // Vfx
        public Vfx Vfx => GetRow<Vfx>(VfxId);
    }

}