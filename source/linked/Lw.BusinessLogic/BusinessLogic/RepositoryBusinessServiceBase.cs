using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lw.ComponentModel.Composition;
using Lw.Services;
using Lw.DataAccess;

namespace Lw.BusinessLogic
{
    public class RepositoryBusinessServiceBase<TRepository> : BusinessServiceBase
        where TRepository : RepositoryBase
    {
        #region Public Properties
        /// <summary>
        ///     Gets and sets an instance of <typeparam name="TRepository"/>.
        /// </summary>
        /// <value>
        ///     An instance of <typeparam name="TRepository"/>.
        /// </value>
        /// <remarks>
        ///     <notes type="implementnotes">
        ///         If not set by the constructor or property setter, the value is retrieved from 
        ///         <see cref="Components.Current"/>.
        ///     </notes>
        /// </remarks>
        public TRepository Repository
        {
            get
            {
                return Operations.InitializeIfNull(
                    ref this.repository, () => 
                {
                    var components = Lw.ComponentModel.Composition.Components.Current.GetInstance<TRepository>();
                    this.DisposeDelegate.Disposibles.Add(repository);
                    return components;
                });
            }
            set
            {
                if (this.repository != null)
                {
                    this.DisposeDelegate.Disposibles.Remove(this.repository);
                }

                this.repository = value;
                this.DisposeDelegate.Disposibles.Add(this.repository);
            }
        }
        #endregion Public Properties


        #region Protected Constructors
        /// <summary>
        ///     Initializes a new <see cref="DbContextBusinessServiceBase"/> instance. Properties will be 
        ///     initialized from <see cref="Component.Current"/> on first access.
        /// </summary>
        /// <remarks>
        ///     <see cref="Repository"/> will be disposed by the <see cref="DbContextBusinessServiceBase"/>.
        /// </remarks>
        protected RepositoryBusinessServiceBase()
            : this(null, null)
        {

        }

        /// <summary>
        ///     Initializes a new <see cref="DbContextBusinessServiceBase"/> instance.
        /// </summary>
        /// <param name="coreServices">
        ///     An <see cref="ICoreServices"/> implementation.
        /// </param>
        /// <param name="repository">
        ///     An <typeparamref name="TRepository"/>.
        /// </param>
        /// <remarks>
        ///     <paramref name="repository"/> will be disposed by the 
        ///     <see cref="RepositoryBusinessServiceBase{TRepository}"/>.
        /// </remarks>
        protected RepositoryBusinessServiceBase(ICoreServices coreServices, TRepository repository)
            : base(coreServices)
        {
            this.Repository = repository;
        }
        #endregion Protected Constructors

        #region Protected Methods
        protected TResponse InvokeTransaction<TRequest, TResponse>(
                Func<TRepository, Func<TRequest, TResponse>> repositoryTransaction, TRequest request)
            where TResponse : class
        {
            TResponse response = null;

            this.DoWithExceptionPolicy(() =>
            {
                response = repositoryTransaction(this.Repository)(request);
            });

            return response;
        }

        protected IList<TSummary> GetSummariesByCriteria<TCriteria, TSummary>(
            Func<TRepository, Func<TCriteria, IList<TSummary>>> repositoryQuery, TCriteria criteria)
        {
            IList<TSummary> summaries  = null;

            this.DoWithExceptionPolicy(() =>
            {
                summaries = repositoryQuery(this.Repository)(criteria);
            });

            return summaries;
        }
        #endregion Protected Methods

        #region Private Fields
        private TRepository repository;
        #endregion Private Fields
    }
}
