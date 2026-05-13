using Simulant.Core.Entity;
using System;
using System.Numerics;

namespace Simulant.Simulation.Runtime
{
    public sealed class SimCharacter : Character
    {
        public SimCharacter() : base()
        { 
        }

        public Vector2? TargetPos { get; set; }
        public float Speed { get; set; } = 6.0f;
    }
}
