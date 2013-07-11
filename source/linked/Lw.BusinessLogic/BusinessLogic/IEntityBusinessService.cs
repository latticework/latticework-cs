using System.Collections.Generic;
using Lw.BusinessEntities;

namespace Lw.BusinessLogic
{
    public interface IEntityBusinessService
    {
        IList<IBusinessEntity> GetByKey(IEnumerable<IBusinessEntityKey> keys);
    }

    public interface IEntityBusinessService<TEntity, TEntityKey>
        : IEntityBusinessService
        where TEntity : IBusinessEntity
        where TEntityKey : IBusinessEntityKey
    {
        IList<TEntity> GetByKey(IEnumerable<TEntityKey> keys);
    }
}
