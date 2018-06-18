using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;

namespace Rx.AspNet.EntityFramework
{
    public interface ISqlDbContext
    {
        Database Database { get; }

        DbConnection DbConnection { get;}

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}
