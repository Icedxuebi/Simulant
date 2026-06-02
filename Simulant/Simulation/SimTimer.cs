using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Simulant.Core;

namespace Simulant.Simulation
{
    // 这是一个开发初期用的工具类。to do: 可以自动控制时间执行 Delegate 的 Timeline
    public sealed class SimTimer : IDisposable
    {
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly PluginHost _host;

        private bool _disposed;

        public SimTimer(PluginHost host = null)
        {
            _host = host;
        }

        public double Since
            => _stopwatch.ElapsedMilliseconds / 1000.0;

        public long SinceMs
            => _stopwatch.ElapsedMilliseconds;

        public Task WaitUntil(double seconds)
            => WaitUntil(seconds, _cts.Token);

        public async Task WaitUntil(double seconds, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            if (seconds < 0)
            {
                _host?.LogWarning($"SimTimer: WaitUntil 传入了负数时间 {seconds}，已自动调整为 0");
                seconds = 0;
            }
            var remainingMs = seconds * 1000.0 - _stopwatch.Elapsed.TotalMilliseconds;

            if (remainingMs > 0)
                await Task.Delay((int)remainingMs, cancellationToken);
            else
                _host?.LogWarning($"SimTimer: WaitUntil 预期等待至 {seconds} 秒，但初始已超时至 {Since:0.000} 秒，未等待");
        }

        public void Schedule(double seconds, Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            ThrowIfDisposed();
            RunScheduledAction(seconds, action, _cts.Token);
        }

        private async void RunScheduledAction(double seconds, Action action, CancellationToken cancellationToken)
        {
            try
            {
                await WaitUntil(seconds, cancellationToken);

                if (!cancellationToken.IsCancellationRequested)
                    action();
            }
            catch (OperationCanceledException)
            {
                // expected when Cancel / Dispose is called
            }
            catch (Exception ex)
            {
                _host?.LogError($"排队的动作执行出错 @ {seconds:0.000} s: {ex}");
            }
        }

        public void Cancel()
        {
            if (_disposed)
                return;

            _cts.Cancel();
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(SimTimer));
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _cts.Cancel();
            _cts.Dispose();
            _disposed = true;
        }
    }
}