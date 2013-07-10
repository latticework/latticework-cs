using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data;

namespace Lw.Data.Entity
{
    /// <summary>
    ///     An IDbContext interface represents a combination of the Unit Of Work and Repository
    ///     patterns such that it can be used to query from a database and group together
    ///     changes that will then be written back to the store as a unit.  
    ///     Application's DbContext must inherit this Interface.
    /// </summary>
    public interface IDbContext : IDisposable
    {
        EntityState GetEntityState(object entity);

        MetadataWorkspace GetMetadata();

        /// <summary>
        ///     Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>The number of objects written to the underlying database.</returns>
        /// <exception cref="System.InvalidOperationException">
        ///     Thrown if the context has been disposed.
        /// </exception>
        int SaveChanges();

        /// <summary>
        ///     Returns a DbSet instance for access to entities of the given type in the context, the 
        ///     <see cref="ObjectStateManager"/>, and the underlying store.
        /// </summary>
        /// <typeparam name="TEntity">
        ///     The type entity for which a set should be returned.
        /// </typeparam>
        /// <returns>
        ///     A set for the given entity type.
        /// </returns>
        /// <remarks>
        ///     See the <see cref="IDbSet"/> interface for more details.
        /// </remarks>
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        void SetEntityState(object entity, EntityState state);

    }
}