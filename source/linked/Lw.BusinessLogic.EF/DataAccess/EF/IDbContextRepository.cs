using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lw.Data.Entity;
using Lw.Services;

namespace Lw.DataAccess.EF
{
    public interface IDbContextRepository<TDbContext> : IDisposable
        where TDbContext : IDbContext
    {
        TDbContext Context { get; set; }

        IList<TSummary> GetSummariesByCriteria<TCriteria, TSummary>(
            Func<TDbContext, Func<TCriteria, IList<TSummary>>> contextQuery, TCriteria criteria);
    }
}
