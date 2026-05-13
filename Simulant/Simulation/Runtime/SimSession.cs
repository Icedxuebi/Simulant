using Simulant.Core;
using System;

namespace Simulant.Simulation.Runtime
{
    public sealed class SimSession : IDisposable
    {
        private readonly PluginHost _host;
        private readonly SimLogicBase _logic;

        public bool IsRunning { get; private set; }

        internal SimSession(PluginHost host, SimPresetBase preset)
        {
            _host = host;
            _logic = (SimLogicBase)Activator.CreateInstance(preset.SimLogicType);
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
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
