using Simulant.Core.Entity;
using NativeCharacter = Simulant.Game.FFCS.Client.Game.Character.Character;
using System;
using System.Numerics;
using Simulant.Core;

namespace Simulant.Simulation.Runtime
{
    public sealed class SimCharacter : Character
    {
        public SimCharacter(NativeCharacter native, PluginHost host) : base(native, host)
        { 
        }

        public Vector2? TargetPos { get; set; }
        public float Speed { get; set; } = 6.0f;
    }
}
