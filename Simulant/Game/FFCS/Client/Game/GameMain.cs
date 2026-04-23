using System;
using System.Numerics;

namespace Simulant.Game.FFCS.Client.Game
{
    // Client::Game::GameMain
    public struct GameMain : IMemoryObject
    {
        public IntPtr Ptr { get; set; }

        [SigPattern("48 8D 0D * * * * 0F 28 F2 48 89 44 24 ?")]
        public static IntPtr InstancePtr { get; set; }
        public static GameMain Instance => InstancePtr.As<GameMain>();
        public MemoryField<uint> CurrentTerritoryTypeId => Ptr.Field<uint>(0x4108);
        public MemoryField<ushort> CurrentContentFinderConditionId => Ptr.Field<ushort>(0x4114);

        // from Hyperborea
        [SigPattern("40 55 56 41 54 41 56 41 57 48 81 EC ? ? ? ? 48 8B 05 ? ? ? ? 48 33 C4 48 89 44 24")]
        public static IntPtr LoadZoneFuncPtr { get; set; }
        public IntPtr LoadZone(uint territoryId, int storyProgress = 0, byte a4 = 0, byte a5 = 0, byte a6 = 1)
            => LoadZoneFuncPtr.Call<IntPtr>(Ptr, territoryId, storyProgress, a4, a5, a6);

        [SigPattern("E8 * * * * 8D 46 0A")]
        public static IntPtr ExecuteCommandFuncPtr { get; set; }
        public static bool ExecuteCommand(int command, int param1 = 0, int param2 = 0, int param3 = 0, int param4 = 0)
            => ExecuteCommandFuncPtr.Call<bool>(command, param1, param2, param3, param4);

        [SigPattern("E8 * * * * 49 8D 54 24 ? B9")]
        public static IntPtr ExecuteLocationCommandFuncPtr { get; set; }
        public static bool ExecuteLocationCommand(IntPtr vector3Ptr, int command, int param1 = 0, int param2 = 0, int param3 = 0, int param4 = 0)
            => ExecuteLocationCommandFuncPtr.Call<bool>(command, vector3Ptr, param1, param2, param3, param4);

        public struct Festival : IMemoryObject
        {
            public IntPtr Ptr { get; set; }
            public MemoryField<ushort> Id => Ptr.Field<ushort>(0x00);
            public MemoryField<ushort> Phase => Ptr.Field<ushort>(0x02);
        }
    }
}