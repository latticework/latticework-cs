using System;
using System.Data.Entity;
using System.Linq;
using Lw.Data.Entity;
using Lw.Diagnostics;
using Lw.Services;
using Lw.DataAccess.EF;
using System.Collections.Generic;

namespace Lw.BusinessLogic.EF
{
    public abstract class EFAggregateBusinessServiceBase<TDbContext, TAggregateRoot, TEFAggregateRepository>
        : AggregateBusinessServiceBase<TEFAggregateRepository, TAggregateRoot>
        where TDbContext : class, IDbContext
        where TAggregateRoot : class
        where TEFAggregateRepository : EFAggregateRepository<TDbContext, TAggregateRoot>
    {
        #region Protected Constructors
        /// <summary>
        ///     Initializes a new <see cref="DbContextBusinessServiceBase"/> instance. Properties will be 
        ///     initialized from <see cref="Component.Current"/> on first access.
        /// </summary>
        /// <remarks>
        ///     <see cref="Context"/> will be disposed by the <see cref="DbContextBusinessServiceBase"/>.
        /// </remarks>
        protected EFAggregateBusinessServiceBase()
            : this(null, null)
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
        ///     <paramref name="dbContext"/> will be disposed by the <see cref="DbContextBusinessServiceBase"/>.
        /// </remarks>
        protected EFAggregateBusinessServiceBase(ICoreServices coreServices, TEFAggregateRepository repository)
            : base(coreServices, repository)
        {
        }
        #endregion Protected Constructors
    }

    public class EFAggregateBusinessServiceBase<TDbContext, TAggregateRoot>
    : EFAggregateBusinessServiceBase<TDbContext, TAggregateRoot, EFAggregateRepository<TDbContext, TAggregateRoot>>
        where TDbContext : class, IDbContext
        where TAggregateRoot : class
    {
        #region Protected Constructors
        /// <summary>
        ///     Initializes a new <see cref="DbContextBusinessServiceBase"/> instance. Properties will be 
        ///     initialized from <see cref="Component.Current"/> on first access.
        /// </summary>
        /// <remarks>
        ///     <see cref="Context"/> will be disposed by the <see cref="DbContextBusinessServiceBase"/>.
        /// </remarks>
        protected EFAggregateBusinessServiceBase()
            : this(null, null)
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
        ///     <paramref name="dbContext"/> will be disposed by the <see cref="DbContextBusinessServiceBase"/>.
        /// </remarks>
        protected EFAggregateBusinessServiceBase(
                ICoreServices coreServices, EFAggregateRepository<TDbContext, TAggregateRoot> repository)
            : base(coreServices, repository)
        {
        }
        #endregion Protected Constructors
    }
}
