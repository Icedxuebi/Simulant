using Simulant.Core;
using System;

namespace Simulant.Simulation.Runtime
{
    public sealed class SimSession : IDisposable
    {
        private readonly PluginHost _host;
        private readonly SimLogicBase _logic;
        private readonly SimEntityManager _entityManager;

        public bool IsRunning { get; private set; }

        internal SimSession(PluginHost host, SimPresetBase preset)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
            _ = preset ?? throw new ArgumentNullException(nameof(preset));
            _ = preset.SimLogicType ?? throw new NullReferenceException($"{preset.GetType().Name} 预设类型未定义 SimLogicType，或 SimLogicType 为 null。");

            if (!typeof(SimLogicBase).IsAssignableFrom(preset.SimLogicType))
                throw new InvalidOperationException($"{preset.GetType().Name} 预设的逻辑类型 {preset.SimLogicType.Name} 不继承自 SimLogicBase。");

            _entityManager = new SimEntityManager(_host.EntitySpawner);

            _logic = (SimLogicBase)Activator.CreateInstance(preset.SimLogicType, _host, preset);
            _logic.EntityManager = _entityManager;
        }

        public void Start()
        {
            if (IsRunning)
                throw new InvalidOperationException("SimSession 已在运行中。");

            IsRunning = true;
            _logic.Start();
        }

        public void Stop()
        {
            if (!IsRunning) return;
            IsRunning = false;
            _logic.Stop();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
