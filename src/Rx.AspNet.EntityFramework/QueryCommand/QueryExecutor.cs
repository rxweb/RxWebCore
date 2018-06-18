using System.Collections.Generic;
using System.Linq;

namespace Rx.AspNet.EntityFramework
{
    public class QueryExecutor : IQueryExecutor 
    {
        private IDbContext Context { get; set; }
        public QueryExecutor(IDbContext context) {
            Context = context;
        }
        private IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : class {
            return Context.Set<TEntity>().AsQueryable();
        }

        public IEnumerable<TEntity> Execute<TEntity>(QueryObject<TEntity> queryObject) where TEntity : class {
            return queryObject.ToQuery(AsQueryable<TEntity>()).ToList();
        }
    }
}
