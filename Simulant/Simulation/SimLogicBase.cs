using Simulant.Simulation.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Simulant.Simulation
{
    public abstract class SimLogicBase
    {
        private readonly SimPresetBase _preset;

        /// <summary> 需要使用的不可见假实体数量，用于产生技能特效。</summary>
        public abstract int DummyCount { get; }
        public SimEntityManager EntityManager { get; set; }

        public SimLogicBase(SimPresetBase preset)
        {
            _preset = preset;
        }

        public void Start()
        { 
            LoadOptionsFrom(_preset.Options);
            EntityManager.CreateDummies(DummyCount);
            SimLogic();
        }

        public void Stop()
        {
            EntityManager.Clear();
        }

        public abstract void SimLogic();

        internal void LoadOptionsFrom(IEnumerable<SimOptionBase> options)
        {
            // 传入的所有 UI 选项构建为字典，过滤掉非 SimOption<T> 子类（没有 PropertyName）
            var dict = (options ?? Enumerable.Empty<SimOptionBase>())
                .Where(o => !string.IsNullOrEmpty(o.PropertyName))
                .ToDictionary(o => o.PropertyName);

            // 当前 SimLogicBase 子类中所有标记 SimOptionAttribute 的属性
            var props = GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(p => p.GetCustomAttribute<SimOptionAttribute>() != null);

            foreach (var prop in props)
            {
                if (!dict.TryGetValue(prop.Name, out var option))
                    throw new InvalidOperationException($"配置选项 {GetType().Name} 的标记属性 {prop.Name} 在界面中未找到对应选项。");

                option.ApplyTo(this);
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SimOptionAttribute : Attribute
    {
    }
}
