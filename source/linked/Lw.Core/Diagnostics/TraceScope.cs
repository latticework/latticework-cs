using System;

namespace Lw.Diagnostics
{
    public abstract class TraceScope : IDisposable
    {
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !disposed)
            {
                disposed = true;
            }
        }

        private bool disposed = false;
    }
}
