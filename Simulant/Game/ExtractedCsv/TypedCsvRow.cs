using System;
using System.Collections.Generic;

namespace Simulant.Game.ExtractedCsv
{
    public abstract class TypedCsvRow : CsvRow
    {
        public abstract string Name { get; }

        public TRow GetRow<TRow>(int id) where TRow : TypedCsvRow
        {
            return Manager.Get<TRow>()[id];
        }

    }

}
