using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lw.Data.Entity;
using Lw.ComponentModel.Composition;
using Lw.Services;
using Lw.Diagnositcs;

namespace Lw.DataAccess.EF
{
    public class DbContextRepository<TDbContext> : RepositoryBase, IDbContextRepository<TDbContext>
        where TDbContext : class, IDbContext
    {
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


        #region Public Constructors
        /// <summary>
        ///     Initializes a new <see cref="DbContextBusinessServiceBase"/> instance. Properties will be 
        ///     initialized from <see cref="Component.Current"/> on first access.
        /// </summary>
        /// <remarks>
        ///     <see cref="Context"/> will be disposed by the <see cref="DbContextRepository{TDbContext}"/>.
        /// </remarks>
        public DbContextRepository(string repositoryExceptionPolicy = 
                EntityFrameworkExceptionPolicy.DefaultExceptionPolicy)
            : this(null, null, repositoryExceptionPolicy)
        {

        }

        /// <summary>
        ///     Initializes a new <see cref="DbContextBusinessServiceBase"/> instance.
        /// </summary>
        /// <param name="coreServices">
        ///     An <see cref="ICoreServices"/> implementation.
        /// </param>
        /// <param name="dbContext">
        ///     An <typeparamref name="TDbContext"/>.
        /// </param>
        /// <remarks>
        ///     <paramref name="dbContext"/> will be disposed by the <see cref="DbContextRepository{TDbContext}"/>.
        /// </remarks>
        public DbContextRepository(
            ICoreServices coreServices, 
            TDbContext dbContext, 
            string repositoryExceptionPolicy = EntityFrameworkExceptionPolicy.DefaultExceptionPolicy)
            : base(coreServices, repositoryExceptionPolicy)
        {
            this.Context = dbContext;
        }
        #endregion Public Constructors

        #region Private Fields
        private TDbContext dbContext;
        #endregion Private Fields
    }
}
