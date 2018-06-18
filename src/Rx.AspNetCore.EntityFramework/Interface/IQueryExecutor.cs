using System.Collections.Generic;

namespace Rx.AspNetCore.EntityFramework
{
    public interface IQueryExecutor
    {
        IEnumerable<TEntity> Execute<TEntity>(QueryObject<TEntity> queryObject) where TEntity : class;
    }
}
