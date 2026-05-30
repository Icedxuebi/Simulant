using System;

namespace Simulant.Game.FFCS.Client.Game.Character
{
    public struct CharacterData : IMemoryObject
    {
        public IntPtr Ptr { get; set; }

        public MemoryField<float> ModelScale => Ptr.Field<float>(0x8);
        public MemoryField<uint> Health => Ptr.Field<uint>(0xC);
        public MemoryField<uint> MaxHealth => Ptr.Field<uint>(0x10);
        public MemoryField<uint> Mana => Ptr.Field<uint>(0x14);
        public MemoryField<uint> MaxMana => Ptr.Field<uint>(0x18);
        public MemoryField<ushort> GatheringPoints => Ptr.Field<ushort>(0x1C);
        public MemoryField<ushort> MaxGatheringPoints => Ptr.Field<ushort>(0x1E);
        public MemoryField<ushort> CraftingPoints => Ptr.Field<ushort>(0x20);
        public MemoryField<ushort> MaxCraftingPoints => Ptr.Field<ushort>(0x22);
        public MemoryField<short> TransformationId => Ptr.Field<short>(0x24);
        public MemoryField<ushort> TitleId => Ptr.Field<ushort>(0x26);
        public MemoryField<ushort> StatusLoopVfxId => Ptr.Field<ushort>(0x28);
        public MemoryField<Job> ClassJob => Ptr.Field<Job>(0x2A);
        public MemoryField<byte> Level => Ptr.Field<byte>(0x2B);
        public MemoryField<byte> Icon => Ptr.Field<byte>(0x2C);
        public MemoryField<byte> SEPack => Ptr.Field<byte>(0x2D);
        public MemoryField<byte> ShieldValue => Ptr.Field<byte>(0x2E);
        public MemoryField<byte> Map => Ptr.Field<byte>(0x2F);
        public MemoryField<byte> OnlineStatus => Ptr.Field<byte>(0x30);
        public MemoryField<byte> Battalion => Ptr.Field<byte>(0x31);

        public MemoryField<byte> Flags => Ptr.Field<byte>(0x34);
        public MemoryBitField IsHostile => Ptr.BitField(0x34, 0);
        public MemoryBitField InCombat => Ptr.BitField(0x34, 1);
    }

    public enum Job : byte
    {
        ADV = 0, GLA = 1, PGL = 2, MRD = 3, LNC = 4, ARC = 5, CNJ = 6, THM = 7,
        CRP = 8, BSM = 9, ARM = 10, GSM = 11, LTW = 12, WVR = 13, ALC = 14, CUL = 15,
        MIN = 16, BTN = 17, FSH = 18, PLD = 19, MNK = 20, WAR = 21, DRG = 22, BRD = 23,
        WHM = 24, BLM = 25, ACN = 26, SMN = 27, SCH = 28, ROG = 29, NIN = 30, MCH = 31,
        DRK = 32, AST = 33, SAM = 34, RDM = 35, BLU = 36, GNB = 37, DNC = 38, RPR = 39,
        SGE = 40, VPR = 41, PCT = 42, _43 = 43, _44 = 44,
    }
}