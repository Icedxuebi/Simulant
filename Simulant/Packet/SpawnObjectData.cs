using Simulant.Game.FFCS.Client.Game.Object;

namespace Simulant.Packet
{
    public struct SpawnObjectData
    {
        public byte ObjectIndex;
        public ObjectKind ObjectKind;
        public byte TargetableStatus;
        public byte Visibility;

        public uint Id;
        public uint BaseId;
        public uint EntityId;
        public uint LayoutId;
        public uint OwnerId;
        public uint GimmickId;

        public float Radius;
        public float X;
        public float Y;
        public float Z;
        public float Heading;

        public ushort FateId;
        public byte EventState;
    }
}