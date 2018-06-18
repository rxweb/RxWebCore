using System.Linq;

namespace Rx.AspNetCore.EntityFramework
{
    public abstract class QueryObject<TEntity>
    {
        public abstract IQueryable<TEntity> ToQuery(IQueryable<TEntity> queryableEntity);
    }
}
