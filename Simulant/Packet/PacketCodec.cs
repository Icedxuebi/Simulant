using System;

namespace Simulant.Game
{
    public static class PacketCodec
    {
        private const double CoordScale = 3.0518043 * 0.0099999998;
        private const double HeadingScale = 0.009587526 * 0.0099999998;
        private const double TwoPi = Math.PI * 2.0;

        /// <summary>
        /// Decodes a packet coordinate encoded as UInt16.
        /// </summary>
        public static float DecodeUShortCoord(ushort value)
        {
            return (float)(value * CoordScale - 1000.0);
        }

        /// <summary>
        /// Encodes an in-game coordinate into the packet UInt16 format.
        /// Coordinates are clamped to the representable packet range.
        /// </summary>
        public static ushort EncodeUShortCoord(float coord)
        {
            var raw = (int)Math.Round((coord + 1000.0) / CoordScale);

            if (raw < 0)
                raw = 0;
            else if (raw > ushort.MaxValue)
                raw = ushort.MaxValue;

            return (ushort)raw;
        }

        /// <summary>
        /// Decodes a packet heading encoded as UInt16 into an in-game heading.
        /// </summary>
        public static float DecodeUShortHeading(ushort heading)
        {
            return (float)(heading * HeadingScale - Math.PI);
        }

        /// <summary>
        /// Encodes an in-game heading into the packet UInt16 format.
        /// Heading is wrapped by 2π instead of clamped.
        /// </summary>
        public static ushort EncodeUShortHeading(float heading)
        {
            var normalized = (heading + Math.PI) % TwoPi;
            if (normalized < 0)
                normalized += TwoPi;

            var raw = (int)Math.Round(normalized / HeadingScale);
            return (ushort)(raw & 0xFFFF);
        }
    }
}