using System;

namespace Simulant.ACT
{
    /// <summary>
    /// Wrapper for GreyMagic.FrameLockRelease.
    /// </summary>
    public sealed class GreyMagicFrameLockRelease : IDisposable
    {
        private readonly dynamic _release;
        private bool _disposed;

        public object RawRelease => _release;

        public GreyMagicFrameLockRelease(object frameLockRelease)
        {
            _release = frameLockRelease ?? throw new ArgumentNullException(nameof(frameLockRelease));
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _release.Dispose();
            _disposed = true;
        }
    }
}
