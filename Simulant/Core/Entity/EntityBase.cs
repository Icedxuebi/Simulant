using System;
using System.Numerics;
using Simulant.Game;
using Simulant.Game.FFCS.Client.Game.Object;

namespace Simulant.Core.Entity
{
    public abstract class EntityBase
    {
        protected abstract GameObject NativeGameObject { get; }
        public GameObject Native => NativeGameObject;

        public IntPtr Address => NativeGameObject.Ptr;

        public uint Id
        {
            get => Native.EntityId;
            set => Native.EntityId.Set(value);
        }

        public float X
        {
            get => Native.Position.Ptr.Read<float>(0x0);
            set => Native.Position.Ptr.Write(value, 0x0);
        }

        // Flip Y and Z to match the ACT coord system
        public float Y
        {
            get => Native.Position.Ptr.Read<float>(0x8);
            set => Native.Position.Ptr.Write(value, 0x8);
        }

        public float Z
        {
            get => Native.Position.Ptr.Read<float>(0x4);
            set => Native.Position.Ptr.Write(value, 0x4);
        }

        public Vector2 Pos
        {
            get => new Vector2(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Vector3 Pos3D
        {
            get => new Vector3(X, Y, Z);
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
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