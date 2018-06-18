using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace Rx.AspNet.EntityFramework
{
    public interface IDbContext:IDisposable
    {
        Database Database { get; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbEntityEntry<T> Entry<T>(T entity) where T : class;
        int SaveChanges();

        DbChangeTracker ChangeTracker { get; }
    }
}
