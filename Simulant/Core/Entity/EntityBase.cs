using Simulant.Game;
using Simulant.Game.FFCS.Client.Game.Object;
using Simulant.Simulation.Runtime;
using System;
using System.Numerics;

namespace Simulant.Core.Entity
{
    public abstract class EntityBase
    {
        protected abstract GameObject NativeGameObject { get; }
        public GameObject Native => NativeGameObject;

        public SimEntityState SimState { get; set; }

        public IntPtr Address => NativeGameObject.Ptr;

        public uint Id
        {
            get => Native.EntityId;
            set => Native.EntityId.Set(value);
        }

        public float X
        {
            get => Native.Position.Ptr.Read<float>(0x0);
            set
            {
                Native.Position.Ptr.Write(value, 0x0);
                Native.PositionModified();
            }
        }

        // Flip Y and Z to match the ACT coord system
        public float Y
        {
            get => Native.Position.Ptr.Read<float>(0x8);
            set
            {
                Native.Position.Ptr.Write(value, 0x8);
                Native.PositionModified();
            }
        }

        public float Z
        {
            get => Native.Position.Ptr.Read<float>(0x4);
            set
            {
                Native.Position.Ptr.Write(value, 0x4);
                Native.PositionModified();
            }
        }

        // 尽量降低读写次数
        public Vector2 Pos
        {
            get
            {
                Vector3 rawPos = Native.Position;
                return new Vector2(rawPos.X, rawPos.Z);
            }
            set
            {
                Vector3 rawPos = Native.Position;
                Native.Position.Set(new Vector3(value.X, rawPos.Y, value.Y));
                Native.PositionModified();
            }
        }

        // 尽量降低读写次数
        public Vector3 Pos3D
        {
            get
            {
                Vector3 rawPos = Native.Position;
                return new Vector3(rawPos.X, rawPos.Z, rawPos.Y);
            }
            set
            {
                Native.Position.Set(new Vector3(value.X, value.Z, value.Y));
                Native.PositionModified();
            }
        }

        public float Heading
        {
            get => Native.Rotation;
            set
            {
                Native.Rotation.Set(value);
                Native.RotationModified();
            }
        }

        public float Scale
        {
            get => Native.Scale;
            set => Native.Scale.Set(value);
        }

        public void EnableDraw() => Native.EnableDraw();
        public void DisableDraw() => Native.DisableDraw();
        public void SetReadyToDraw() => Native.SetReadyToDraw();
        public void Redraw()
        {
            DisableDraw();
            EnableDraw();
        }
    }
}