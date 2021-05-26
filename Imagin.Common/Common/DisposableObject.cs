using System;

namespace Imagin.Common
{
    /// <summary>
    /// Specifies an <see cref="object"/> that implements <see cref="IDisposable"/>.
    /// </summary>
    public abstract class DisposableObject : Base, IDisposable
    {
        bool disposed = false;

        public DisposableObject()
        {
        }

        ~DisposableObject()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
                OnManagedDisposed();

            OnUnmanagedDisposed();

            disposed = true;
        }

        /// <summary>
        /// Occurs when managed resources need disposed.
        /// </summary>
        protected virtual void OnManagedDisposed() { }

        /// <summary>
        /// Occurs when unmanaged resources need disposed.
        /// </summary>
        protected virtual void OnUnmanagedDisposed() { }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}