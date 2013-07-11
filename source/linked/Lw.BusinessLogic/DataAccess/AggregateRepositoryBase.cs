using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lw.Diagnostics;
using Lw.Services;

namespace Lw.DataAccess
{
    public abstract class AggregateRepositoryBase<TAggregateRoot> : RepositoryBase
        where TAggregateRoot : class
    {
        #region Public Properties
        IQueryable<TAggregateRoot> AggregateSetQuery { get { return this.SelectAggregates(this.GetAggregateRoots()); } }
        #endregion Public Properties

        #region Public Methods
        public virtual IList<TAggregateRoot> GetAggregatesByQuery(
            Func<IQueryable<TAggregateRoot>, IQueryable<TAggregateRoot>> aggregateSelector)
        {
            IList<TAggregateRoot> aggregates = null;

            this.DoWithExceptionPolicy(() =>
            {
                aggregates = aggregateSelector(this.AggregateSetQuery).ToList();
            });

            return aggregates;
        }

        public virtual void NewAggregates(IEnumerable<TAggregateRoot> aggregates)
        {
            this.DoWithExceptionPolicy(() =>
            {
                this.NewAggregatesCore(aggregates);
                this.CommitAggregates(aggregates);
            });
        }

        public virtual void RemoveAggregateByQuery(
            Func<IQueryable<TAggregateRoot>, IQueryable<TAggregateRoot>> aggregatesSelector)
        {
            this.DoWithExceptionPolicy(() =>
            {
                var aggregates = aggregatesSelector(this.AggregateSetQuery).ToList();

                this.RemoveAggregatesCore(aggregates);
                this.CommitAggregates(aggregates);
            });
        }

        public virtual void SetAggregates(IEnumerable<TAggregateRoot> aggregates)
        {
            this.DoWithExceptionPolicy(() =>
            {
                var storeAggregates = this.SelectStoreAggregates(aggregates, this.AggregateSetQuery).ToList();

                this.SetAggregatesCore(aggregates, storeAggregates);
                this.CommitAggregates(storeAggregates);
            });
        }
        #endregion Public Methods


        #region Protected Constructors
        /// <summary>
        ///     Initializes a new <see cref="AggregateRepositoryBase"/> instance. Properties will be 
        ///     initialized from <see cref="Component.Current"/> on first access.
        /// </summary>
        protected AggregateRepositoryBase(
                Func<IQueryable<TAggregateRoot>, IQueryable<TAggregateRoot>> aggregateSelector = null,
                Func<IQueryable<TAggregateRoot>, IEnumerable<TAggregateRoot>, IQueryable<TAggregateRoot>> 
                    storeAggregateSelector = null,
                string repositoryExceptionPolicy = RepositoryBase.DefaultRepositoryExceptionPolicy)
            : this(
                null, aggregateSelector, storeAggregateSelector, repositoryExceptionPolicy)
        {
        }

        /// <summary>
        ///     Initializes a new <see cref="AggregateRepositoryBase"/> instance.
        /// </summary>
        /// <param name="coreServices">
        ///     An <see cref="ICoreServices"/> implementation.
        /// </param>
        /// <param name="repository">
        ///     An <typeparamref name="TRepository"/>.
        /// </param>
        protected AggregateRepositoryBase(
                ICoreServices coreServices,
                Func<IQueryable<TAggregateRoot>, IQueryable<TAggregateRoot>> aggregateSelector = null,
                Func<IQueryable<TAggregateRoot>, IEnumerable<TAggregateRoot>, IQueryable<TAggregateRoot>> 
                    storeAggregateSelector = null,
                string repositoryExceptionPolicy = RepositoryBase.DefaultRepositoryExceptionPolicy)
            : base(coreServices, repositoryExceptionPolicy)
        {
            this.AggregateSelector = aggregateSelector;
            this.StoreAggregateSelector = storeAggregateSelector;
        }
        #endregion Protected Constructors

        #region Protected Properties
        protected Func<IQueryable<TAggregateRoot>, IQueryable<TAggregateRoot>> AggregateSelector { get; set; }

        protected Func<IQueryable<TAggregateRoot>, IEnumerable<TAggregateRoot>, IQueryable<TAggregateRoot>>
            StoreAggregateSelector { get; set; }
        #endregion  Protected Properties

        #region Protected Methods
        protected abstract void CommitAggregates(IEnumerable<TAggregateRoot> aggregates);

        protected abstract void NewAggregatesCore(IEnumerable<TAggregateRoot> aggregates);

        protected abstract void RemoveAggregatesCore(IEnumerable<TAggregateRoot> aggregates);

        protected abstract IQueryable<TAggregateRoot> GetAggregateRoots();

        protected virtual IQueryable<TAggregateRoot> SelectAggregates(IQueryable<TAggregateRoot> rootSet)
        {
            if (this.AggregateSelector != null)
            {
                return this.AggregateSelector(rootSet);
            }

            return rootSet;
        }

        protected virtual IQueryable<TAggregateRoot> SelectStoreAggregates(
            IEnumerable<TAggregateRoot> aggregates, IQueryable<TAggregateRoot> rootSet)
        {
            if (this.StoreAggregateSelector != null)
            {
                return this.StoreAggregateSelector(rootSet, aggregates);
            }

            return rootSet;
        }

        protected abstract void SetAggregatesCore(
            IEnumerable<TAggregateRoot> aggregates, IEnumerable<TAggregateRoot> storeAggregates);
        #endregion Protected Methods
    }
}
