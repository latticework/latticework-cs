using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lw
{
    public class DisposableDelegate : IDisposable
    {
        #region Public Constructors
        /// <summary>
        ///     Initializes a new instance.
        /// </summary>
        /// <param name="parent">
        ///     An object inherited from <see cref="IDisposable"/> which uses DisposableDelegate.
        /// </param>
        public DisposableDelegate(IDisposable parent) 
        {
            this.parent = parent;
            this.IsDisposed = false;
            this.Disposibles = new HashSet<IDisposable>();
            this.UnmanagedResources = new HashSet<Action>();
        }
        #endregion Public Constructors

        #region Public Properties
        /// <summary>
        ///     Gets the set of IDisposable objects.
        /// </summary>
        /// <value>
        ///     A set specifying the list of objects inherited from IDisposable interface to be released by 
        ///     <see cref="Dispose()"/>.
        /// </value>
        public ISet<IDisposable> Disposibles { get; private set; }

        /// <summary>
        ///     Gets or sets the flag denoting whether the requested object has been disposed or not.
        /// </summary>
        /// <value>
        ///     A boolean value specifying if the concerned object has already been disposed.
        /// </value>
        public bool IsDisposed { get; set; }

        /// <summary>
        ///     Gets the set of unmanaged resources used in parent.
        /// </summary>
        /// <value>
        ///     A set of delegates to execute when <see cref="Dispose()"/> is called or the finalizer is called.
        /// </value>
        public ISet<Action> UnmanagedResources { get; private set; }
        #endregion Public Properties

        #region Public Methods
        /// <summary>
        ///     Implementation of Dispose as contracted by <see cref="IDisposable"/>.
        ///     Call this for disposing managed resources.
        /// </summary>
        public void Dispose()
        {
            try
            {
                this.Dispose(disposing: true);
            }
            finally
            {
                GC.SuppressFinalize(this);
                GC.SuppressFinalize(parent);
            }
        }

        
        /// <summary>
        ///     Call this for disposing unmanaged resources.
        /// </summary>
        ~DisposableDelegate()
        {
            this.Dispose(disposing: false);
        }

        #endregion Public Methods

        #region Protected Methods
        protected virtual void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                try
                {
                    if (disposing)
                    {
                        foreach (var disposable in this.Disposibles)
                        {
                            try
                            {
                                disposable.Dispose();
                            }
                            catch { }
                        }
                    }

                    foreach (var unmanagedResource in this.UnmanagedResources)
                    { 
                        try
                        {
                            unmanagedResource();
                        }
                        catch { }
                    }
                }
                finally
                {
                    this.Disposibles.Clear();
                    this.UnmanagedResources.Clear();
                    this.IsDisposed = true;
                }
            }
        }
        #endregion Protected Methods

        #region Private Fields
        private readonly IDisposable parent;
        #endregion Private Fields
    }
}
