using Simulant.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Simulant.Simulation.Runtime
{
    public class SimEntityState
    {
        /// <summary> 该模拟状态所属的假实体。</summary>
        public EntityBase Entity { get; private set; }

        /// <summary> 该实体是否为模拟实体（假实体）。</summary>
        public bool IsSimulated { get; private set; }

        /// <summary> 实体在模拟中的职能序号（若为玩家或假玩家）。</summary>
        public int? RoleIndex { get; set; }

        public Vector2? MoveTarget { get; set; }
        public float MoveSpeed { get; set; } = 6f;

        public SimEntityState(EntityBase entity)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
        }
    }
}