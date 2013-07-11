using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lw.ComponentModel.Composition;
using Lw.Services;
using Lw.Diagnostics;

namespace Lw.DataAccess
{
    public abstract class RepositoryBase : ComponentBase, IDisposable
    {
        #region Public Fields
        public const string DefaultRepositoryExceptionPolicy = "RepositoryExceptionPolicy";
        #endregion Public Fields


        #region Public Properties
        /// <summary>
        ///     Gets the source <see cref="IComponentContainer"/> for the view model.
        /// </summary>
        /// <value>
        ///     An <see cref="IComponentContainer"/>.
        /// </value>
        public IComponentContainer Components { get { return this.CoreServices.Components; } }

        /// <summary>
        ///     Gets the exception manager service.
        /// </summary>
        /// <value>
        ///     An <see cref="IExceptionManager"/>.
        /// </value>
        public IExceptionManager Exceptions { get { return this.CoreServices.ExceptionManager; } }

        /// <summary>
        ///     Gets the ILogWriter service.
        /// </summary>
        /// <value>
        ///     An <see cref="ILogWriter"/>.
        /// </value>
        public ILogWriter Logs { get { return this.CoreServices.LogWriter; } }

        /// <summary>
        ///     Gets the security service.
        /// </summary>
        /// <value>
        ///     An <see cref="ISecurityService"/>.
        /// </value>
        public ISecurityService Security { get { return this.CoreServices.SecurityService; } }
        #endregion Public Properties

        #region Public Methods
        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or 
        ///     resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.disposeDelegate.Dispose();
        }

        public void DoWithExceptionPolicy(Action action)
        {
            DoWithExceptionPolicy(suppressExceptionPolicy: false, action: action);
        }
        #endregion Public Methods


        #region Protected Constructors
        /// <summary>
        ///     Initializes a new <see cref="BusinessServiceBase"/> instance. Properties will be 
        ///     initialized from <see cref="Component.Current"/> on first access.
        /// </summary>
        protected RepositoryBase(string repositoryExceptionPolicy = RepositoryBase.DefaultRepositoryExceptionPolicy)
            : this(null, repositoryExceptionPolicy)
        {
        }

        /// <summary>
        ///     Initializes a new <see cref="BusinessServiceBase"/> instance.
        /// </summary>
        /// <param name="coreServices">
        ///     An <see cref="ICoreServices"/> implementation.
        /// </param>
        protected RepositoryBase(
            ICoreServices coreServices, 
            string repositoryExceptionPolicy = RepositoryBase.DefaultRepositoryExceptionPolicy)
            : base(coreServices)
        {
            this.disposeDelegate = new DisposableDelegate(this);
            this.RepositoryExceptionPolicy = repositoryExceptionPolicy;
        }
        #endregion Protected Constructors

        #region Protected Properties
        /// <summary>
        ///     Gets the object that manages disposal for contained disposable objects when <see cref="Dispose"/> is
        ///     called.
        /// </summary>
        /// <value>
        ///     A <see cref="DisposableDelegate"/>.
        /// </value>
        protected DisposableDelegate DisposeDelegate { get { return this.disposeDelegate; } }

        protected string RepositoryExceptionPolicy { get; private set; }
        #endregion Protected Properties

        #region Protected Methods
        protected void DoWithExceptionPolicy(bool suppressExceptionPolicy, Action action)
        {
            if (!suppressExceptionPolicy)
            {
                this.Exceptions.DoWithExceptionHandling(this.RepositoryExceptionPolicy, () => { action(); });
            }
            else
            {
                action();
            }
        }
        #endregion Protected Methods


        #region Private Fields
        private DisposableDelegate disposeDelegate;
        #endregion Private Fields
    }
}
