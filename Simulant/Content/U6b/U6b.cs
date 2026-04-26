using System.Linq;
using Simulant.Core.Environment;

namespace Simulant.Content.U6b
{
    [SimTerritory(1122)]
    public static class U6b
    {
        [Phase(10)]
        public static readonly PhaseData P1;
        [Phase(20)]
        public static readonly PhaseData P2;
        [Phase(30)]
        public static readonly PhaseData P3;
        [Phase(40)]
        public static readonly PhaseData P4;
        [Phase(50)]
        public static readonly PhaseData P5;
        [Phase(60)]
        public static readonly PhaseData P6;

        static U6b()
        {
            var mapEffects = Enumerable.Repeat<ushort>(0x4, 0x16).ToList();
            mapEffects[0] = 0x1; // 电网
            P1 = new PhaseData("P1", 77, 0, mapEffects);
            P2 = new PhaseData("P2", 78, 0, mapEffects);
            P3 = new PhaseData("P3", 79, 0, mapEffects);
            P4 = new PhaseData("P4", 0, 0, mapEffects);
            P5 = new PhaseData("P5", 0, 0, mapEffects);
            P6 = new PhaseData("P6", 0, 0, mapEffects);
        }
    }
}
