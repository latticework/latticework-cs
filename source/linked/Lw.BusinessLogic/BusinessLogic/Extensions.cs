using System;

using Lw.BusinessEntities;

namespace Lw.BusinessLogic
{
    /// <summary>
    ///     Extension methods for <c>Lw.BusinessLogic</c> types.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///     Retrieves the entity with the specified key from the service.
        /// </summary>
        /// <typeparam name="TEntity">
        ///     The entity type.
        /// </typeparam>
        /// <typeparam name="TEntityKey">
        ///     The entity key type.
        /// </typeparam>
        /// <param name="service">
        ///     The business service.
        /// </param>
        /// <param name="key">
        ///     The unique identity for the specified entity type.
        /// </param>
        /// <returns>
        ///     The specified entity or <see langword="null"/> if the key was not found.
        /// </returns>
        public static TEntity GetByKey<TEntity, TEntityKey>(
                this IEntityBusinessService<TEntity, TEntityKey> service, TEntityKey key)
            where TEntity : class, IBusinessEntity
            where TEntityKey : IBusinessEntityKey
        {
            var entities = service.GetByKey(new TEntityKey[] { key });

            if (entities == null || entities.Count > 1)
            {
                throw new InvalidOperationException("GetByKey must return the same number of elements that are passed in.");
            }

            return entities[0];
        }

        /// <summary>
        ///     Retrieves the entity with the specified key from the service.
        /// </summary>
        /// <param name="service">
        ///     The business service.
        /// </param>
        /// <param name="key">
        ///     The unique identity for the specified entity type.
        /// </param>
        /// <returns>
        ///     The specified entity or <see langword="null"/> if the key was not found.
        /// </returns>
        public static IBusinessEntity GetByKey(this IEntityBusinessService service, IBusinessEntityKey key)
        {
            var entities = service.GetByKey(new IBusinessEntityKey[] { key });

            if (entities == null || entities.Count > 1)
            {
                throw new InvalidOperationException("GetByKey must return the same number of elements that are passed in.");
            }

            return entities[0];
        }
    }
}
