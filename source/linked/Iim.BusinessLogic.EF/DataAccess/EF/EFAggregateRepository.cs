using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lw.ComponentModel.Composition;
using Lw.Services;
using Lw.Data.Entity;
using System.Data.Entity;
using Lw.Diagnositcs;
using Lw.Linq.Expressions;

namespace Lw.DataAccess.EF
{

    public class EFAggregateRepository<TDbContext, TAggregateRoot>
        : AggregateRepositoryBase<TAggregateRoot>, IDbContextRepository<TDbContext>
        where TDbContext : class, IDbContext
        where TAggregateRoot : class
    {
        #region Public Constructors
        /// <summary>
        ///     Initializes a new <see cref="AggregateRepositoryBase"/> instance. Properties will be 
        ///     initialized from <see cref="Component.Current"/> on first access.
        /// </summary>
        public EFAggregateRepository(
                Func<IQueryable<TAggregateRoot>, IQueryable<TAggregateRoot>> aggregateSelector = null,
                Action<IDbSet<TAggregateRoot>, IEnumerable<TAggregateRoot>> markAggregatesAsAddedAction = null,
                Action<IDbSet<TAggregateRoot>, IEnumerable<TAggregateRoot>> markAggregatesAsModifiedAction = null,
                Action<IDbSet<TAggregateRoot>, IEnumerable<TAggregateRoot>> markAggregatesAsRemovedAction = null,
                string repositoryExceptionPolicy = EntityFrameworkExceptionPolicy.DefaultExceptionPolicy)
            : this(
                null,
                null,
                aggregateSelector,
                markAggregatesAsAddedAction,
                markAggregatesAsModifiedAction,
                markAggregatesAsRemovedAction,
                repositoryExceptionPolicy)
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
        public EFAggregateRepository(
                ICoreServices coreServices,
                TDbContext context, 
                Func<IDbSet<TAggregateRoot>, IQueryable<TAggregateRoot>> aggregateSelector = null,
                Action<IDbSet<TAggregateRoot>, IEnumerable<TAggregateRoot>> markAggregatesAsAddedAction = null,
                Action<IDbSet<TAggregateRoot>, IEnumerable<TAggregateRoot>> markAggregatesAsModifiedAction = null,
                Action<IDbSet<TAggregateRoot>, IEnumerable<TAggregateRoot>> markAggregatesAsRemovedAction = null,
                string repositoryExceptionPolicy = EntityFrameworkExceptionPolicy.DefaultExceptionPolicy)
            : base(
                coreServices,
                ToBaseAggregateSelector(aggregateSelector),
                ToBaseStoreAggregateSelector(context),
                repositoryExceptionPolicy)
        {
            this.Context = context;

            this.MarkAggregatesAsAddedAction = markAggregatesAsAddedAction;
            this.MarkAggregatesAsModifiedAction = markAggregatesAsModifiedAction;
            this.MarkAggregatesAsRemovedAction = markAggregatesAsRemovedAction;
        }
        #endregion Public Constructors

        #region Public Properties
        /// <summary>
        ///     Gets and sets an instance of <typeparam name="TDbContext"/>.
        /// </summary>
        /// <value>
        ///     An instance of <typeparam name="TDbContext"/>.
        /// </value>
        /// <remarks>
        ///     <notes type="implementnotes">
        ///         If not set by the constructor or property setter, the value is retrieved from 
        ///         <see cref="Components.Current"/>.
        ///     </notes>
        /// </remarks>
        public TDbContext Context
        {
            get
            {

                return Operations.InitializeIfNull(
                    ref this.dbContext, () =>
                    {
                        var components = Lw.ComponentModel.Composition.Components.Current.GetInstance<TDbContext>();
                        this.DisposeDelegate.Disposibles.Add(dbContext);
                        return components;
                    });
            }
            set
            {
                if (this.dbContext != null)
                {
                    this.DisposeDelegate.Disposibles.Remove(this.dbContext);
                }

                this.dbContext = value;
                this.DisposeDelegate.Disposibles.Add(this.dbContext);
            }
        }
        #endregion Public Properties

        #region Public Methods
        public IList<TSummary> GetSummariesByCriteria<TCriteria, TSummary>(
            Func<TDbContext, Func<TCriteria, IList<TSummary>>> contextQuery, TCriteria criteria)
        {
            IList<TSummary> summaries = null;

            this.DoWithExceptionPolicy(() =>
            {
                summaries = contextQuery(this.Context)(criteria);
            });

            return summaries;
        }
        #endregion Public Methods


        #region Protected Properties
        public IEqualityComparer<TAggregateRoot> AggregateComparer { get; set; }

        protected IDbSet<TAggregateRoot> DbSet { get { return this.Context.Set<TAggregateRoot>(); } }

        protected Action<IDbSet<TAggregateRoot>, IEnumerable<TAggregateRoot>> MarkAggregatesAsAddedAction
        { get; set; }

        protected Action<IDbSet<TAggregateRoot>, IEnumerable<TAggregateRoot>> MarkAggregatesAsRemovedAction
        { get; set; }

        protected Action<IDbSet<TAggregateRoot>, IEnumerable<TAggregateRoot>> MarkAggregatesAsModifiedAction
        { get; set; }
        #endregion Protected Properties

        #region Protected Methods
        protected override void CommitAggregates(IEnumerable<TAggregateRoot> aggregates)
        {
            this.Context.SaveChanges();
        }

        protected override IQueryable<TAggregateRoot> GetAggregateRoots()
        {
            return this.DbSet;
        }

        protected override void NewAggregatesCore(IEnumerable<TAggregateRoot> aggregates)
        {
            if (this.MarkAggregatesAsAddedAction != null)
            {
                this.MarkAggregatesAsAddedAction(this.DbSet, aggregates);
            }

            this.DbSet.Add(aggregates);
        }

        protected override void RemoveAggregatesCore(IEnumerable<TAggregateRoot> aggregates)
        {
            if (this.MarkAggregatesAsRemovedAction != null)
            {
                this.MarkAggregatesAsRemovedAction(this.DbSet, aggregates);
            }

            this.Context.AttachRemoved(aggregates);
        }

        protected override void SetAggregatesCore(
            IEnumerable<TAggregateRoot> aggregates, IEnumerable<TAggregateRoot> storeAggregates)
        {
            if (this.MarkAggregatesAsModifiedAction != null)
            {
                this.MarkAggregatesAsModifiedAction(this.DbSet, aggregates);
            }

            this.Context.Merge(aggregates, storeAggregates);

            this.Context.AttachModified(storeAggregates);
        }
        #endregion Protected Methods


        #region Private Fields
        private TDbContext dbContext;
        #endregion Private Fields

        #region Private Methods
        private static Func<IQueryable<TAggregateRoot>, IQueryable<TAggregateRoot>> ToBaseAggregateSelector(
            Func<IDbSet<TAggregateRoot>, IQueryable<TAggregateRoot>> aggregateSelector)
        {
            if (aggregateSelector == null) { return null; }

            return query => aggregateSelector((IDbSet<TAggregateRoot>)query);
        }

        private static Func<IQueryable<TAggregateRoot>, IEnumerable<TAggregateRoot>, IQueryable<TAggregateRoot>>
            ToBaseStoreAggregateSelector(TDbContext context)
        {
            return (query, aggregates) => query.Where(context.BuildEntityInPredicate(aggregates));
        }
        #endregion Private Methods
    }
}
