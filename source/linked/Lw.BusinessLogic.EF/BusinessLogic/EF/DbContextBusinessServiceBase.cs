using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lw.Data.Entity;
using Lw.ComponentModel.Composition;
using Lw.Services;
#if DOTNET45  
using System.Data.Objects;
using System.Data.Metadata.Edm;
#endif
#if DOTNET451
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Metadata.Edm;
#endif
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Lw.DataAccess.EF;

namespace Lw.BusinessLogic.EF
{
    /// <summary>
    ///     Provides a base class for Business Service implementations that rely on a <see cref="IDbContext"/> 
    ///     implementation for persistence of business entities or to peform business transactions.
    /// </summary>
    /// <typeparam name="TDbContext">
    ///     The <see cref="IDbContext"/> relied upon.
    /// </typeparam>
    public abstract class DbContextBusinessServiceBase<TDbContext, TDbContextRepository>
        : RepositoryBusinessServiceBase<TDbContextRepository>
        where TDbContext : class, IDbContext
        where TDbContextRepository : DbContextRepository<TDbContext>
    {
        #region Protected Constructors
        /// <summary>
        ///     Initializes a new <see cref="DbContextBusinessServiceBase"/> instance. Properties will be 
        ///     initialized from <see cref="Component.Current"/> on first access.
        /// </summary>
        /// <remarks>
        ///     <see cref="Context"/> will be disposed by the <see cref="DbContextBusinessServiceBase"/>.
        /// </remarks>
        protected DbContextBusinessServiceBase()
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
        protected DbContextBusinessServiceBase(ICoreServices coreServices, TDbContextRepository repository)
            : base(coreServices, repository)
        {
        }
        #endregion Protected Constructors
    }

    /// <summary>
    ///     Provides a base class for Business Service implementations that rely on a <see cref="IDbContext"/> 
    ///     implementation for persistence of business entities or to peform business transactions.
    /// </summary>
    /// <typeparam name="TDbContext">
    ///     The <see cref="IDbContext"/> relied upon.
    /// </typeparam>
    public abstract class DbContextBusinessServiceBase<TDbContext>
        : DbContextBusinessServiceBase<TDbContext, DbContextRepository<TDbContext>>
        where TDbContext : class, IDbContext
    {
        #region Protected Constructors
        /// <summary>
        ///     Initializes a new <see cref="DbContextBusinessServiceBase"/> instance. Properties will be 
        ///     initialized from <see cref="Component.Current"/> on first access.
        /// </summary>
        /// <remarks>
        ///     <see cref="Context"/> will be disposed by the <see cref="DbContextBusinessServiceBase"/>.
        /// </remarks>
        protected DbContextBusinessServiceBase()
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
        protected DbContextBusinessServiceBase(ICoreServices coreServices, DbContextRepository<TDbContext> repository)
            : base(coreServices, repository)
        {
        }
        #endregion Protected Constructors
    }
}
