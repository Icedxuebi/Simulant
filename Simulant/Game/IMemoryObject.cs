using Simulant.Game.ExtractedCsv.Rows;
using System;

namespace Simulant.Game
{
    public interface IMemoryObject
    {
        IntPtr Ptr { get; set; }
    }

    public static class MemoryObjectExtensions
    {
        public static bool IsNull<TMemoryObject>(this TMemoryObject obj)
            where TMemoryObject : struct, IMemoryObject
        {
            return obj.Ptr == IntPtr.Zero;
        }
    }
}