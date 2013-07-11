using System;
using System.Collections.Generic;
using System.Linq;
using Lw.BusinessEntities;

namespace Lw.BusinessLogic
{
    public abstract class EntityBusinessServiceBase<TEntity, TEntityKey>
        : IEntityBusinessService<TEntity, TEntityKey>
        where TEntity : IBusinessEntity
        where TEntityKey : IBusinessEntityKey
    {
        public abstract IList<TEntity> GetByKey(IEnumerable<TEntityKey> keys);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException">
        ///     An element in the sequence cannot be cast to type {TEntityKey}.
        /// </exception>
        IList<IBusinessEntity> IEntityBusinessService.GetByKey(IEnumerable<IBusinessEntityKey> keys)
        {
            // This can be replaced in C#4.0 because of covariance.
            return GetByKey(keys.Cast<TEntityKey>()).Cast<IBusinessEntity>().ToList();
        }
    }
}
