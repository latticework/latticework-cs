using System;
using System.Linq;
using Lw.Diagnostics;
using Lw.Services;
using System.Collections.Generic;
using Lw.DataAccess;

namespace Lw.BusinessLogic
{
    public abstract class AggregateBusinessServiceBase<TAggregateRepository, TAggregateRoot>
        : RepositoryBusinessServiceBase<TAggregateRepository>
        where TAggregateRoot : class
        where TAggregateRepository : AggregateRepositoryBase<TAggregateRoot>
    {
        #region Protected Constructors
        /// <summary>
        ///     Initializes a new <see cref="DbContextBusinessServiceBase"/> instance. Properties will be 
        ///     initialized from <see cref="Component.Current"/> on first access.
        /// </summary>
        /// <remarks>
        ///     <see cref="Context"/> will be disposed by the <see cref="DbContextBusinessServiceBase"/>.
        /// </remarks>
        protected AggregateBusinessServiceBase()
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
        ///     <paramref name="repository"/> will be disposed by the <see cref="DbContextBusinessServiceBase"/>.
        /// </remarks>
        protected AggregateBusinessServiceBase(ICoreServices coreServices, TAggregateRepository repository)
            : base(coreServices, repository)
        {
        }
        #endregion Protected Constructors

        #region Protected Properties
        protected string RepositoryExceptionPolicy { get; set; }
        #endregion  Protected Properties

        #region Protected Methods
        protected IList<TAggregateRoot> GetAggregatesByQuery(
            Func<IQueryable<TAggregateRoot>, 
            IQueryable<TAggregateRoot>> aggregateSelector, 
            bool suppressExceptionPolicy = false)
        {
            IList<TAggregateRoot> aggregates = null;

            this.DoWithExceptionPolicy(suppressExceptionPolicy: suppressExceptionPolicy, action: () =>
            {
                aggregates = this.GetAggregatesByQueryCore(aggregateSelector);
            });

            return aggregates;
        }

        protected virtual IList<TAggregateRoot> GetAggregatesByQueryCore(
            Func<IQueryable<TAggregateRoot>, IQueryable<TAggregateRoot>> aggregateSelector)
        {
            return this.Repository.GetAggregatesByQuery(aggregateSelector);
        }

        protected void NewAggregates(
            IEnumerable<TAggregateRoot> aggregates, bool suppressExceptionPolicy = false)
        {
            this.DoWithExceptionPolicy(suppressExceptionPolicy: suppressExceptionPolicy, action: () =>
            {
                this.NewAggregatesCore(aggregates);
            });
        }

        protected virtual void NewAggregatesCore(IEnumerable<TAggregateRoot> aggregates)
        {
            this.Repository.NewAggregates(aggregates);
        }

        protected void RemoveAggregateByQuery(
            Func<IQueryable<TAggregateRoot>, IQueryable<TAggregateRoot>> aggregatesSelector, 
            bool suppressExceptionPolicy = false)
        {
            this.DoWithExceptionPolicy(suppressExceptionPolicy: suppressExceptionPolicy, action: () =>
            {
                this.RemoveAggregateByQueryCore(aggregatesSelector);
            });
        }

        protected virtual void RemoveAggregateByQueryCore(
            Func<IQueryable<TAggregateRoot>, IQueryable<TAggregateRoot>> aggregatesSelector)
        {
            this.Repository.RemoveAggregateByQuery(aggregatesSelector);
        }

        protected void SetAggregates(
            IEnumerable<TAggregateRoot> aggregates, bool suppressExceptionPolicy = false)
        {
            this.DoWithExceptionPolicy(suppressExceptionPolicy: suppressExceptionPolicy, action: () =>
            {
                this.SetAggregatesCore(aggregates);
            });
        }

        protected virtual void SetAggregatesCore(IEnumerable<TAggregateRoot> aggregates)
        {
            this.Repository.SetAggregates(aggregates);
        }

        
        #endregion Protected Methods


    }
}
