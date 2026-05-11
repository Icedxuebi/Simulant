using Simulant.Core.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulant.Simulation
{
    public abstract class SimPresetBase
    {
        public abstract int TerritoryId { get; }
        public abstract string Name { get; }
        public abstract string Author { get; }
        public abstract DateTime LastUpdated { get; }
        public abstract PhaseData Phase { get; }
        public abstract Type SimLogicType { get; }
        public abstract string Description { get; }

        /// <summary> 全部 Option，包括纯 Label 这种仅用于显示 UI 的假 Option。</summary>
        public List<SimOptionBase> Options { get; protected set; } = new List<SimOptionBase>();

        /// <summary> 设置了 PropertyName 的 Option，应该均为绑定属性的 <see cref="SimOption{T}"/>。 </summary>
        public Dictionary<string, SimOptionBase> NamedOptions { get; protected set; } = new Dictionary<string, SimOptionBase>();

        public override string ToString() => $"[模拟] {Name}";
        public SimLogicBase CreateSimLogic()
        {
            // TODO: Add error handling for invalid SimLogicType
            return SimLogicType == null ? null : (SimLogicBase)Activator.CreateInstance(SimLogicType);
        }

        protected void AddOption(SimOptionBase option)
        {
            if (option == null) throw new NullReferenceException(nameof(option));
            Options.Add(option);
            if (!string.IsNullOrEmpty(option.PropertyName))
                NamedOptions[option.PropertyName] = option;
        }

        internal void ApplyOptions()
        { 
            foreach (var option in NamedOptions.Values)
            {
                option.ApplyTo(this);
            }
        }
    }
}
