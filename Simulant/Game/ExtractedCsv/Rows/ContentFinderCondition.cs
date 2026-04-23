using System;
using System.Collections.Generic;

namespace Simulant.Game.ExtractedCsv.Rows
{
    public class ContentFinderCondition : TypedCsvRow
    {

        // "AAC Cruiserweight M1"
        public override string Name => Get("Name");
        public uint ContentId => Get<uint>("Content");
        public byte ContentTypeId => Get<byte>("ContentType"); // ContentType
        // Raids
        public ContentType ContentType => GetRow<ContentType>(ContentTypeId);

        // many other boolean fields
    }

}