using System;

namespace Simulant.ACT
{
    /// <summary>
    /// Wrapper for GreyMagic.FrameLock.
    /// </summary>
    public sealed class GreyMagicFrameLock : IDisposable
    {
        private readonly dynamic _lock;
        private bool _disposed;

        public object RawLock => _lock;

        public GreyMagicFrameLock(object frameLock)
        {
            _lock = frameLock ?? throw new ArgumentNullException(nameof(frameLock));
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _lock.Dispose();
            _disposed = true;
        }
    }
}
